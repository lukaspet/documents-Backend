using Quartz;
using System.IO;
using System.Net.Mail;
using System.Net;
using System;
using System.Configuration;
using System.Web;
using System.Threading.Tasks;
using WebApi10Min.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi10Min.Helpers.Email;

namespace WebApi10Min.Helpers.Schedulers
{
    [DisallowConcurrentExecution] // This attribute prevents Quartz.NET from trying to run the same job concurrently.
    public class EmailJob : IJob
    {
        private readonly IServiceScopeFactory scopeFactory; // Since DbContext is scoped by default, you need to create scope to access it.It also allows you to handle its lifetime correctly - otherwise you'd keep instance of DbContext for a long time and this is not recommended.
        private readonly IMailService mailService;

        public EmailJob(IServiceScopeFactory scopeFactory, IMailService mailService)
        {
            this.scopeFactory = scopeFactory;
            this.mailService = mailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                var dbAuth = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                var users = await dbAuth.Aspnetusers.ToListAsync();
                var documents = await db.Document.Where(x => x.DataScadenza > DateTime.Now && x.DataScadenza < DateTime.UtcNow.AddDays(30)).ToListAsync(); // search for documents that have dataScadenza today
                if (documents.Count > 0)
                {
                    var subscribes = await db.UserSubscribe.ToListAsync();
                    var subsriberList = new System.Collections.Generic.List<string>(); // create a helper list which is used to create emails for multiple users

                    foreach (var subscribe in subscribes)
                    {
                        if (!subsriberList.Contains(subscribe.UserId))
                        {
                            subsriberList.Add(subscribe.UserId);

                            var subsubscribe = await db.UserSubscribe.Where(x => x.UserId == subscribe.UserId).ToListAsync();
                            var user = users.FirstOrDefault(x => x.Id == subscribe.UserId).Email;
                            var bodyJoins = documents.Where(y => subsubscribe.Any(z => z.DocumentId == y.DocumentId)).ToList();
                            var body = "<h1>Documenti in scadenza: </h1>";

                            foreach (var bdj in bodyJoins)
                            {
                                body += "<br> <b>id: <b>" + bdj.DocumentId + "</b>,   Descrizione documento: <b>" + bdj.DescrizioneDocumento + "</b>,  Scadenza documento: <b>" + bdj.DataScadenza.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy") + "</b>";
                            }
                            // var body = " " + Environment.NewLine + string.Join(Environment.NewLine, bodyJoin.Select(x => x.DataScadenza));
                            try
                            {
                                MailRequest request = new MailRequest
                                {
                                    ToEmail = user,
                                    Subject = "[Zanutta Documentazione]:Documenti in scadenza dal" + DateTime.UtcNow.ToString("dd/MM/yyyy") + " al " + DateTime.UtcNow.AddDays(30).ToString("dd/MM/yyyy"),
                                    Body = body
                                };

                                await mailService.SendEmailAsync(request);
                                return;
                            }
                            catch (Exception ex)
                            {
                                MailRequest request = new MailRequest
                                {
                                    ToEmail = "lspetic@zanuttaspa.it",
                                    Subject = "Error in mail ZanuttaDocumentazione",
                                    Body = "Error happend when trying to send email with error: " + ex
                                };
                                await mailService.SendEmailAsync(request);
                                throw;
                            }
                        }
                    }
                }
                
            }
        }
    }
}
