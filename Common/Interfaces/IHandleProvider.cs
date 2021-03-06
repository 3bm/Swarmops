using System;

namespace Swarmops.Common.Interfaces
{
    public interface IHandleProvider
    {
        string GetPersonHandle (int personId);
        void SetPersonHandle (int personId, string newHandle);
        HandleErrorType CanSetHandle (string newHandle);

        int GetPersonByHandle (string handle);
    }

    public enum HandleErrorType
    {
        Unknown = 0,
        NoError,
        HandleNotFound,
        HandleOccupied
    }

    public class HandleException : Exception
    {
        private readonly string attemptedHandle;
        private readonly HandleErrorType errorType;

        public HandleException (string attemptedHandle, HandleErrorType errorType)
        {
            this.attemptedHandle = attemptedHandle;
            this.errorType = errorType;
        }

        public HandleErrorType ErrorType
        {
            get { return this.errorType; }
        }

        public override string ToString()
        {
            return "HandleException: Handle '" + this.attemptedHandle + "' caused '" + this.errorType + "'.\r\n" +
                   base.ToString();
        }
    }
}