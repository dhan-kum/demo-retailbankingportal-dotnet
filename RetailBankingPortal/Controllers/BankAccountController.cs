using Microsoft.AspNetCore.Mvc;
using RetailBankingPortal.Data;
using RetailBankingPortal.DTOs;
using RetailBankingPortal.Entities;
using RetailBankingPortal.Services;
using System.IO.Compression;

namespace RetailBankingPortal.Controllers;

[ApiController]
[Route("api/bankaccounts")]
public class BankAccountController : ControllerBase
{
    private readonly BankingDbContext _dbContext;
    private readonly TransferService _transferService;
    private readonly FileService _fileService;
    private readonly ConsumerService _consumerService;
    private readonly ILogger<BankAccountController> _logger;
    private const string QueueName = "my_queue";

    public BankAccountController(
        BankingDbContext dbContext,
        TransferService transferService,
        FileService fileService,
        ConsumerService consumerService,
        ILogger<BankAccountController> logger)
    {
        _dbContext = dbContext;
        _transferService = transferService;
        _fileService = fileService;
        _consumerService = consumerService;
        _logger = logger;
    }

    [HttpGet("")]
    public ActionResult<List<BankAccount>> GetAllAccounts()
    {
        _logger.LogInformation("Inside getAllAccounts() method");
        return Ok(_dbContext.BankAccounts.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<BankAccount> GetBankAccount(string id)
    {
        var account = _dbContext.BankAccounts.FirstOrDefault(a => a.AccountNumber == id);
        _logger.LogInformation("Inside getBankAccount() method - {Account}", account);
        if (account == null)
            return NotFound();
        return Ok(account);
    }

    [HttpPost("logmessage")]
    public IActionResult SaveLogs([FromQuery] string logmsg)
    {
        _logger.LogInformation("{LogMessage}", logmsg);
        return Ok();
    }

    [HttpPost("transfer")]
    public ActionResult<string> DoTransfer([FromBody] TransferDTO transferDto)
    {
        var transfer = new Transfer
        {
            SenderAccount = transferDto.SenderAccount,
            ReceiverAccount = transferDto.ReceiverAccount,
            Amount = transferDto.Amount
        };

        try
        {
            var bankAccountCollection = _transferService.ExecuteTransfer(transfer);
            if (bankAccountCollection != null)
            {
                bankAccountCollection.TryGetValue("receiverAccount", out var receiver);
                bankAccountCollection.TryGetValue("senderAccount", out var sender);

                if (receiver != null && sender != null)
                {
                    if (int.Parse(transfer.ReceiverAccount) == int.Parse(receiver.AccountNumber)
                        && int.Parse(transfer.SenderAccount) == int.Parse(sender.AccountNumber))
                    {
                        _logger.LogInformation("Info: Transfer was successful.");
                        return Ok("Transfer was successful.");
                    }
                    else
                    {
                        _logger.LogInformation("Info: Transfer was not successful.");
                    }
                }
                else
                {
                    _logger.LogInformation("Info: Transfer was not successful.");
                }
            }
            else
            {
                _logger.LogInformation("Info: Transfer was not successful.");
            }

            _logger.LogInformation("Transfer money...");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transfer failed");
        }

        return BadRequest();
    }

    [HttpGet("createzip")]
    public ActionResult<string> CreateZip([FromQuery] string sourceDir, [FromQuery] string zipFile)
    {
        try
        {
            using var fileStream = new FileStream(zipFile, FileMode.Create);
            using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create);
            _fileService.AddFilesToZip(sourceDir, Path.GetFileName(sourceDir), archive);
            return Ok("Successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in generating zip");
            return Ok("Error in generating errorMsg-" + e.Message);
        }
    }

    [HttpGet("connect")]
    public IActionResult Send()
    {
        _consumerService.Connect(QueueName);
        return Ok();
    }
}
