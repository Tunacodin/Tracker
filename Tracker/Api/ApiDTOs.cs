namespace Tracker.Api
{
    public class CreateCustomerDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public bool IsMaster { get; set; } = false;
    }

    public class AddAccounToCustomerDTO
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; }
    }

    public class PaginationDTO
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }

    public class CreateMVCLogDTO
    {
        public string Page { get; set; }
        public string? TargetEmail { get; set; }
    }

    public class LogFilterDTO : PaginationDTO
    {
        public string? TargetAccount { get; set; }
        public string? TargetEmail { get; set; }
        public string? LoggedInUserEmail { get; set; }
        public string? Page { get; set; }
        public ActionType? ActionType { get; set; }
        public ProcessStatus? ProcessStatus { get; set; }
    }
}
