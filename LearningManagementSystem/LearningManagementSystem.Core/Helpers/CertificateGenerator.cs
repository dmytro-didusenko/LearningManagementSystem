using LearningManagementSystem.Domain.Models.Certificate;
using System.Text;

namespace LearningManagementSystem.Core.Helpers
{
    public static class CertificateGenerator
    {
        public static string GetHTMLCertificate(CertificateModel certificateModel)
        {
            ArgumentNullException.ThrowIfNull(certificateModel);

            var sb = new StringBuilder();

            sb.Append(@"
                        <html>
                        <head>
                        </head>
                        <body>
                            <div style = 'border: 10px solid #787878; padding:20px; width: 780px; height: 540px; margin: auto'>
                                <div style = 'border: 5px solid #787878; padding:20px'>
                                    <div style = 'text-align: center'>
                                        <span style = 'font-size:60px; font-weight:bold'>Certificate of Completion</span>
                                            <br>
                                            <br>
                                        <span style = 'font-size:40px'><i>This is to certify that</i></span>
                                            <br>
                                            <br>");

            sb.AppendFormat(@"<span style='font-size:45px'><b>{0}</b></span>
                    <br>
                    <br>
                <span style='font-size:40px'><i>has completed the course</i></span>
                    <br>
                    <br>
                <span style='font-size:45px'>{1}</span>
                    <br>
                    <br>
                    <br>
                <span style='font-size:30px'><i>dated</i></span>
                    <br>            
                <span style='font-size:45px'>{2}</span>", certificateModel.StudentName,
                                                          certificateModel.CourseName, 
                                                          certificateModel.Date.ToShortDateString());

            sb.Append(@"
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>");

            return sb.ToString();
        }
    }
}