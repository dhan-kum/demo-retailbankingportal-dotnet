using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailBankingPortal.Data;
using RetailBankingPortal.DTOs;
using RetailBankingPortal.Services;

namespace RetailBankingPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountsController : ControllerBase
{
    private readonly BankingDbContext _context;
    private readonly TransferService _transferService;
    private readonly FileService _fileService;
    private readonly ConsumerService _consumerService;
    private readonly ILogger<BankAccountsController> _logger;

    public BankAccountsController(
        BankingDbContext context,
        TransferService transferService,
        FileService fileService,
        ConsumerService consumerService,
        ILogger<BankAccountsController> logger)
    {
        _context = context;
        _transferService = transferService;
        _fileService = fileService;
        _consumerService = consumerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _context.BankAccounts.ToListAsync();
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(string id)
    {
        var account = await _context.BankAccounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferDTO transferDto)
    {
        var result = await _transferService.TransferFunds(transferDto);
        if (result)
        {
            return Ok(new { message = "Transfer successful" });
        }
        return BadRequest(new { message = "Transfer failed" });
    }

    [HttpPost("logmessage")]
    public IActionResult LogMessage([FromQuery] string logmsg)
    {
        if (string.IsNullOrWhiteSpace(logmsg))
        {
            return BadRequest(new { message = "Log message cannot be empty" });
        }
        
        // Sanitize input to prevent log injection
        var sanitized = logmsg.Replace("\n", " ").Replace("\r", " ");
        _logger.LogInformation("User log message: {Message}", sanitized);
        return Ok(new { message = "Message logged" });
    }

    [HttpGet("createzip")]
    public async Task<IActionResult> CreateZip([FromQuery] string sourceDir, [FromQuery] string zipFile)
    {
        // Validate paths to prevent directory traversal attacks
        if (string.IsNullOrWhiteSpace(sourceDir) || string.IsNullOrWhiteSpace(zipFile))
        {
            return BadRequest(new { message = "Source directory and zip file path are required" });
        }
        
        try
        {
            // Restrict to specific allowed directory
            var baseDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "temp"));
            var fullSourcePath = Path.GetFullPath(Path.Combine(baseDir, sourceDir));
            var fullZipPath = Path.GetFullPath(Path.Combine(baseDir, zipFile));
            
            // Verify paths are within the allowed base directory (case-insensitive for cross-platform support)
            if (!fullSourcePath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase) || 
                !fullZipPath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Invalid path: access denied" });
            }
            
            var result = await _fileService.CreateZipFile(fullSourcePath, fullZipPath);
            if (result)
            {
                return Ok(new { message = "ZIP file created successfully" });
            }
            return BadRequest(new { message = "Failed to create ZIP file" });
        }
        catch (ArgumentException)
        {
            return BadRequest(new { message = "Invalid path format" });
        }
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var result = _consumerService.ConnectToQueue();
        if (result)
        {
            return Ok(new { message = "Connected to RabbitMQ" });
        }
        return BadRequest(new { message = "Failed to connect to RabbitMQ" });
    }
}
