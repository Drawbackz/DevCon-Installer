using System;
using System.ComponentModel;
using DevConInstaller.Core;
using DevConInstaller.Core.Utilities;

namespace DevConInstaller.CommandLine.Commands
{
    public class InstallCommand : Command
    {
        public string Hash { get; private set; }
        public bool UseLatest { get; private set; }
        public bool Update { get; private set; }
        public bool AddToPath { get; private set; }
        public string Directory { get; private set; }
        public SystemArchitecture Architecture { get; private set; }


        public InstallCommand()
        {
            Name = "install";
            Description = "Install DevCon";
            Arguments = new Arguments
            {
                new Argument("--latest", "Use the most recent DevCon version in sources", (value) =>
                {
                    UseLatest = true;
                })
                {
                    Children = new []
                    {
                        new Argument("--architecture", "Define the DevCon architecture variant", (value) =>
                        {
                            if (!Enum.TryParse(value?.ToUpper(), out SystemArchitecture architecture))
                            {
                                throw new InvalidEnumArgumentException("Invalid architecture specified");
                            }
                            Architecture = architecture;
                        }, true)
                    }
                },
                new Argument("--hash", "The download file sha256 hash", (value) =>
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new ArgumentNullException("--hash", "Argument cannot be null or empty");
                    }

                    Hash = value.ToUpper();
                }),
                new Argument("--update", "Update sources if hash not found (limited to 1x every 12 hours)", (value) =>
                {
                    Update = true;
                }),
                new Argument("--addpath", "Add the install directory to the system path (requires administrator access)", (value) =>
                {
                    if (!Permissions.IsAdministrator())
                    {
                        throw new UnauthorizedAccessException("Administrator access is required");
                    }
                    AddToPath = true;
                }),
                new Argument("--dir", "The download and installation directory", (value) =>
                {
                    Directory = value;
                })
            };
        }
    }
}
