namespace Deveplex.TeamFoundation.Build.Activities
{
    using System;
    using System.Activities;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Build.Workflow.Activities;

    public abstract class BuildCodeActivity : CodeActivity, IDisposable
    {
        private bool failingbuild;
        private License license;

        public BuildCodeActivity()
        {
            this.license = LicenseManager.Validate(typeof(BuildCodeActivity), this);
        }

        public void Dispose()
        {
            if (this.license != null)
            {
                this.license.Dispose();
                this.license = null;
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            this.ActivityContext = context;
            try
            {
                this.InternalExecute();
            }
            catch (Exception exception)
            {
                if (!this.failingbuild && this.LogExceptionStack.Get(context))
                {
                    this.LogBuildError(exception.Message + " Stack Trace: " + exception.StackTrace);
                }
                throw;
            }
        }

        protected abstract void InternalExecute();
        protected void LogBuildError(string errorMessage)
        {
            if (this.FailBuildOnError.Get(this.ActivityContext))
            {
                this.failingbuild = true;
                IBuildDetail extension = this.ActivityContext.GetExtension<IBuildDetail>();
                extension.Status = BuildStatus.Failed;
                extension.Save();
                throw new Exception(errorMessage);
            }
            this.ActivityContext.TrackBuildError(errorMessage);
        }

        protected void LogBuildMessage(string message, BuildMessageImportance importance = BuildMessageImportance.Normal)
        {
            this.ActivityContext.TrackBuildMessage(message, importance);
        }

        protected void LogBuildWarning(string warningMessage)
        {
            if (this.TreatWarningsAsErrors.Get(this.ActivityContext))
            {
                this.LogBuildError(warningMessage);
            }
            else
            {
                this.ActivityContext.TrackBuildWarning(warningMessage);
            }
        }

        protected CodeActivityContext ActivityContext { get; set; }

        [Browsable(false), Description("Set to true to fail the build if errors are logged")]
        public InArgument<bool> FailBuildOnError { get; set; }

        [Browsable(false), Description("Set to true to log the entire stack in the event of an exception")]
        public InArgument<bool> LogExceptionStack { get; set; }

        [Browsable(false), Description("Set to true to make all warnings errors")]
        public InArgument<bool> TreatWarningsAsErrors { get; set; }
    }
}

