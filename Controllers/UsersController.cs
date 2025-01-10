
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SecretSantaAPI.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSantaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSenderService _emailSenderService;
        private static readonly object fileLock = new object();

        // Inject IWebHostEnvironment to get the root path of the application
        public UsersController(IWebHostEnvironment environment, IEmailSenderService emailSenderService)
        {
            _environment = environment;
            _emailSenderService = emailSenderService;
        }
        [HttpGet("SetBuddyName")]
        public async Task<IActionResult> SetBuddyName([FromQuery] string email, [FromQuery] string name)
        {

            try
            {
                lock (fileLock)
                {
                    // Load the Excel file using EPPlus
                    using (var package = new ExcelPackage(new FileInfo(Path.Combine(_environment.ContentRootPath, "Data", "SecretSantaData.xlsx"))))
                    {
                        // Get the first worksheet (assuming it's the correct one)
                        var worksheet = package.Workbook.Worksheets["SecretSantaWorksheet"];

                        // Get the total number of rows in the worksheet
                        var rows = worksheet.Dimension.Rows;

                        // Create a dictionary to map emails to row numbers and store user details
                        var users = new Dictionary<string, (int Row, string Name, int IsGotSecretSanta)>();
                        for (int row = 2; row <= rows; row++)  // Assuming row 1 is the header
                        {
                            var userEmail = worksheet.Cells[row, 1].Text; // Emails in column 1
                            var userName = worksheet.Cells[row, 2].Text; // Names in column 2
                            var isGotSecretSanta = int.TryParse(worksheet.Cells[row, 5].Text, out var isGot) ? isGot : 0; // IsGotSecretSanta in column 5
                            if (!string.IsNullOrEmpty(userEmail))
                            {
                                users[userEmail] = (row, userName, isGotSecretSanta);
                            }
                        }

                        if (!users.ContainsKey(email))
                        {
                            return Ok("User not found.");
                        }

                        var userRow = users[email].Row;

                        if (worksheet.Cells[userRow, 4].Value.ToString() != "NA")
                        {
                            return Ok(new
                            {
                                buddyName = worksheet.Cells[userRow, 3].Value.ToString(),
                                buddyEmail = worksheet.Cells[userRow, 4].Value.ToString(),
                                IsAlreadyGot = true
                            });
                        }

                        // Check if the provided email exists


                        // Get the list of remaining users (excluding the current user and those who have IsGotSecretSanta == 1)
                        var remainingUsers = users.Keys.Where(u => u != email && users[u].IsGotSecretSanta == 0).ToList();
                        if (remainingUsers.Count == 0)
                        {
                            return Ok("No other users available to assign as buddies.");
                        }

                        // Select a random buddy
                        var random = new Random();
                        var randomBuddyEmail = remainingUsers[random.Next(remainingUsers.Count)];
                        var randomBuddyRow = users[randomBuddyEmail].Row;
                        var randomBuddyName = users[randomBuddyEmail].Name;

                        // Find the current user's row


                        // Update the BuddyName and BuddyEmail columns for both the current user and the random buddy
                        worksheet.Cells[userRow, 3].Value = randomBuddyName; // BuddyName in column 3
                        worksheet.Cells[userRow, 4].Value = randomBuddyEmail; // BuddyEmail in column 4

                        // Mark both users as having got their Secret Santa // IsGotSecretSanta for the current user (column 5)
                        worksheet.Cells[randomBuddyRow, 5].Value = 1; // IsGotSecretSanta for the buddy

                        // Save the updated Excel file
                        package.Save();
                         _emailSenderService.SendEmail(email, randomBuddyName, randomBuddyEmail, name);

                        return Ok(new
                        {
                            buddyName = randomBuddyName,
                            buddyEmail = randomBuddyEmail,
                            IsAlreadyGot = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }

        [HttpGet("SendEmail")]
        public async Task<IActionResult> SendEmail([FromQuery] string email, string userName)
        {
            try
            {
                await _emailSenderService.SendEmail(email,"Chirag Kapadiya","ckkapadiya@geduservices.com",userName);
                return Ok("Send Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

