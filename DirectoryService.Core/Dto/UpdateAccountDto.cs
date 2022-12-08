namespace DirectoryService.Core.Dto;

public class UpdateAccountDto
{
    public Guid AccountId { get; set; }
    public string Email { get; set; }
    public string PublicKey { get; set; }
    public AccountImagesDto Images { get; set; }
}