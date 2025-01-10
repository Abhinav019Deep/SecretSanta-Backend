using System.Net.Mail;
using System.Net;
using SecretSantaAPI.Interface;

namespace SecretSantaAPI.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        public async Task<bool> SendEmail(string email, string buddyName, string buddyEmail, string userName)
        {
            string secreatSantaHTML= @"<p class=""MsoNormal"" style=""text-align: center; background: #FF6961;"" align=""center""><strong><span
                                                    style=""font-size: 18.0pt; font-family: 'Segoe UI Emoji',sans-serif; mso-fareast-font-family: 'Times New Roman'; mso-bidi-font-family: 'Segoe UI Emoji'; color: white;"">🎅</span></strong><strong><span
                                                    style=""font-size: 18.0pt; font-family: 'Arial',sans-serif; mso-fareast-font-family: 'Times New Roman'; color: white;"">
                                                    Secret Santa 2024 </span></strong><strong><span
                                                    style=""font-size: 18.0pt; font-family: 'Segoe UI Emoji',sans-serif; mso-fareast-font-family: 'Times New Roman'; mso-bidi-font-family: 'Segoe UI Emoji'; color: white;"">🎁</span></strong>
                                        </p>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Arial',sans-serif; color: #333333;"">Hi"+ $" {userName}" +
                                                @",</span></p>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Segoe UI Emoji',sans-serif; mso-bidi-font-family: 'Segoe UI Emoji'; color: #333333;"">🎄</span><span
                                                style=""font-family: 'Arial',sans-serif; color: #333333;""> The holiday season is upon us, and it's time for some
                                                festive fun! You've been selected to participate in our Secret Santa gift exchange. </span><span
                                                style=""font-family: 'Segoe UI Emoji',sans-serif; mso-bidi-font-family: 'Segoe UI Emoji'; color: #333333;"">🎁</span>
                                        </p>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Segoe UI Emoji',sans-serif; mso-bidi-font-family: 'Segoe UI Emoji'; color: #333333;"">Your
                                                Buddy is <span
                                                    style=""font-size: 14pt; color: #e03e2d; font-family: terminal, monaco, monospace;""><strong>"+$" {buddyName} ({buddyEmail}) "+@"</strong></span>.
                                                🥳🥳🥳</span></p>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;"">
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Arial',sans-serif; color: #333333;"">Here are a few important details:</span></p>
                                        <ul type=""disc"">
                                            <li class=""MsoNormal""
                                                style=""color: #333333; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto; mso-list: l0 level1 lfo1; tab-stops: list 36.0pt; background: #F7F7F7;"">
                                                <span style=""font-family: 'Arial',sans-serif; mso-fareast-font-family: 'Times New Roman';"">Budget: <strong><span
                                                            style=""font-family: 'Arial',sans-serif;"">Rs 350</span></strong></span></li>
                                            <li class=""MsoNormal""
                                                style=""color: #333333; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto; mso-list: l0 level1 lfo1; tab-stops: list 36.0pt; background: #F7F7F7;"">
                                                <span style=""font-family: 'Arial',sans-serif; mso-fareast-font-family: 'Times New Roman';"">Gift exchange date:
                                                    <strong><span style=""font-family: 'Arial',sans-serif;"">December 23, 2024</span></strong></span></li>
                                            <li class=""MsoNormal""
                                                style=""color: #333333; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto; mso-list: l0 level1 lfo1; tab-stops: list 36.0pt; background: #F7F7F7;"">
                                                <span style=""font-family: 'Arial',sans-serif; mso-fareast-font-family: 'Times New Roman';"">Location:
                                                    <strong><span style=""font-family: 'Arial',sans-serif;"">Office BreakOut Area</span></strong></span></li>
                                        </ul>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Arial',sans-serif; color: #333333;"">Have fun finding the perfect gift! If you have any
                                                questions, feel free to reach out.</span></p>
                                        <p class=""MsoNormal"" style=""margin-bottom: 7.5pt; background: #F7F7F7;""><span
                                                style=""font-family: 'Arial',sans-serif; color: #333333;"">Happy gifting! </span><span
                                                style=""font-family: 'Segoe UI Emoji',sans-serif; mso-bidi-font-family: 'Segoe UI Emoji'; color: #333333;"">🎁</span>
                                        </p>";
            try
            {
                // Create a new instance of SmtpClient to send email
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Gmail SMTP server port (587 for TLS)
                    Credentials = new NetworkCredential("chirag.kapadiya.geduservices@gmail.com", "dudoufkboivwuhpe"), // Replace with your Gmail credentials
                    EnableSsl = true // Enable SSL for secure connection
                };

                // Create a new email message
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress("ckkapadiya@geduservices.com","Secret Santa"),// Your email address
                    Subject = "Merry Christmas",
                    Sender= new MailAddress("secret.santa@gmail.com"),
                    Body = secreatSantaHTML,
                    IsBodyHtml = true // Set to true if you want to send HTML content
                };

                // Add recipient email address
                mailMessage.To.Add(email); // Replace with the recipient's email address

                // Send the email
                await smtpClient.SendMailAsync(mailMessage);

                // Display a success message
                Console.WriteLine("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
