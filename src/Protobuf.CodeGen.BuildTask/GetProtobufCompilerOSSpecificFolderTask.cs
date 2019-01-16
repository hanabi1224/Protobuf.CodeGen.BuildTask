using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.Framework;

namespace Protobuf.CodeGen.BuildTask
{
    public class GetProtobufCompilerOSSpecificFolderTask : Microsoft.Build.Utilities.Task
    {
        [Output]
        public string FolderName { get; set; }

        public override bool Execute()
        {
            var sb = new StringBuilder();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                sb.Append("windows");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                sb.Append("macosx");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                sb.Append("linux");
            }

            sb.Append('_');
            sb.Append(RuntimeInformation.OSArchitecture.ToString().ToLowerInvariant());

            FolderName = sb.ToString();
            return true;
        }
    }
}
