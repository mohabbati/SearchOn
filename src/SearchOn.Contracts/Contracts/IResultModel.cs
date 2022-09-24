namespace SearchOn.Contracts;

public interface IResultModel
{
    Guid Id { get; set; }

    decimal Score { get; set; }
}
