using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public string BuildOptions { get; set; } = "file_extension=.g.cs";

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
            foreach (var dir in FileNames
                .Select(_ => Path.GetDirectoryName(_)))
            {
                extendedProtoPath.Add(dir);
            }

            var argumentsBuilder = new StringBuilder();
            foreach (var path in extendedProtoPath
                .OrderBy(_ => _))
            {
                argumentsBuilder.Append($" -I {path}");
            }

            if (!string.IsNullOrEmpty(BuildOptions))
            {
                argumentsBuilder.Append($" --csharp_opt={BuildOptions}");
            }

            argumentsBuilder.Append($" --csharp_out={OutputDir}");

            foreach (var file in FileNames
                .OrderBy(_ => _))
            {
                argumentsBuilder.Append($" {file}");
            }

            if (!Directory.Exists(OutputDir))
            {
                Directory.CreateDirectory(OutputDir);
            }

            var command = $"{ProtoCompilerToolPath} {argumentsBuilder.ToString()}";
            Log.LogMessage(MessageImportance.High, command);

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
