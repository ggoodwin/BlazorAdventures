using System.ComponentModel.DataAnnotations;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Requests;
using AutoMapper;
using Domain.Entities.Misc;
using MediatR;
using Shared.Wrapper;

namespace Application.Features.Documents.Commands.AddEdit
{
    public partial class AddEditDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsPublic { get; set; } = false;
        [Required]
        public string URL { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditDocumentCommandHandler : IRequestHandler<AddEditDocumentCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBlobService _blobService;

        public AddEditDocumentCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<Result<int>> Handle(AddEditDocumentCommand command, CancellationToken cancellationToken)
        {
            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"D-{Guid.NewGuid()}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var doc = _mapper.Map<Document>(command);
                if (uploadRequest != null)
                {
                    doc.URL = await _blobService.UploadDocAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Document>().AddAsync(doc);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(doc.Id, "Document Saved");
            }
            else
            {
                var doc = await _unitOfWork.Repository<Document>().GetByIdAsync(command.Id);
                if (doc != null)
                {
                    doc.Title = command.Title ?? doc.Title;
                    doc.Description = command.Description ?? doc.Description;
                    doc.IsPublic = command.IsPublic;
                    if (uploadRequest != null)
                    {
                        doc.URL = await _blobService.UploadDocAsync(uploadRequest);
                    }
                    doc.DocumentTypeId = (command.DocumentTypeId == 0) ? doc.DocumentTypeId : command.DocumentTypeId;
                    await _unitOfWork.Repository<Document>().UpdateAsync(doc);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(doc.Id, "Document Updated");
                }
                else
                {
                    return await Result<int>.FailAsync("Document Not Found!");
                }
            }
        }
    }
}
