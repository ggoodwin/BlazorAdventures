using System.ComponentModel;

namespace Application.Enum
{
    public enum UploadType : byte
    {
        [Description(@"img\Stamps")]
        Stamp,

        [Description(@"img\ProfilePictures")]
        ProfilePicture,

        [Description(@"docs")]
        Document
    }
}
