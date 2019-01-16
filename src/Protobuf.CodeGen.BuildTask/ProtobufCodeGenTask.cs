using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;

namespace Protobuf.CodeGen.BuildTask
{
    public class ProtobufCodeGenTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ProtoCompilerToolPath { get; set; }

        [Required]
        public string[] ProtoPaths { get; set; }

        [Required]
        public string[] FileNames { get; set; }

        [Required]
        public string OutputDir { get; set; }

        [Output]
        public string StdOut { get; set; }

        [Output]
        public string StdError { get; set; }

        public override bool Execute()
        {
            if (!(FileNames?.Length > 0))
            {
                return true;
            }

            var extendedProtoPath = new HashSet<string>(ProtoPaths, StringComparer.OrdinalIgnoreCase);
            foreach (var file in FileNames)
            {
                extendedProtoPath.Add(Path.GetDirectoryName(file));
            }

            var argumentsBuilder = new StringBuilder();
            foreach (var path in extendedProtoPath)
            {
                argumentsBuilder.Append($" -I {path}");
            }

            argumentsBuilder.Append($" --csharp_out={OutputDir}");

            foreach (var file in FileNames)
            {
                argumentsBuilder.Append($" {file}");
            }

            if (!Directory.Exists(OutputDir))
            {
                Directory.CreateDirectory(OutputDir);
            }

            Log.LogMessage(MessageImportance.High, $"{ProtoCompilerToolPath} {argumentsBuilder.ToString()}");

            using (var p = Process.Start(new ProcessStartInfo
            {
                FileName = ProtoCompilerToolPath,
                Arguments = argumentsBuilder.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }))
            {
                p.WaitForExit();

                StdOut = p.StandardOutput.ReadToEnd();
                StdError = p.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(StdOut))
                {
                    Log.LogMessage(MessageImportance.High, StdOut);
                }

                if (!string.IsNullOrEmpty(StdError))
                {
                    Log.LogError(StdError);
                }

                var success = p.ExitCode == 0;

                return success;
            }
        }
    }
}
