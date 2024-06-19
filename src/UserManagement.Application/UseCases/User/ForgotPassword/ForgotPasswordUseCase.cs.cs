using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using UserManagement.Communication.Requests;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.EmailRepository;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.SendEmail;

public class ForgotPasswordUseCase : IForgotPasswordUseCase {

    private readonly ISendEmail _sendEmail;
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordUseCase(
        ISendEmail sendEmail,
        UserManager<Domain.Entities.User> userManager,
        IUnitOfWork unitOfWork
    ){
        _sendEmail = sendEmail;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestEmail request) {

        var user = await Validate(request);

        var passwordToken = RandomStringGeneration(70);

        SendEmailOnForgotPassword(request.Email, passwordToken);

        user!.PasswordToken = passwordToken;
        user.PasswordTokenExpirationDate = DateTime.UtcNow.AddMinutes(10);

        await _userManager.UpdateAsync(user);
        await _unitOfWork.Commit();

    }


    private void SendEmailOnForgotPassword(
        string email,
        string passwordToken
    ){
        var origin = "http://localhost:3000";

        var resetPasswordEmail = $"{origin}/reset-password?token={passwordToken}";

        _sendEmail.Send(
            emailsTo: email,
            subject: "E-mail para recuperação de senha",
            message: $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"format-detection\" content=\"telephone=no\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>E-mail para recuperação de senha</title>\r\n    <style type=\"text/css\">\r\n        #outlook a {{ padding:0; }}\r\n        .ExternalClass {{ width:100%; }}\r\n        .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div {{ line-height: 100%; }}\r\n        table td {{ border-collapse: collapse; mso-line-height-rule: exactly; }}\r\n        .editable.image {{ font-size: 0 !important; line-height: 0 !important; }}\r\n        .nl2go_preheader {{ display: none !important; mso-hide:all !important; mso-line-height-rule: exactly; visibility: hidden !important; line-height: 0px !important; font-size: 0px !important; }}\r\n        body {{ width:100% !important; -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%; margin:0; padding:0; }}\r\n        img {{ outline:none; text-decoration:none; -ms-interpolation-mode: bicubic; }}\r\n        a img {{ border:none; }}\r\n        table {{ border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; }}\r\n        th {{ font-weight: normal; text-align: left; }}\r\n        *[class=\"gmail-fix\"] {{ display: none !important; }}\r\n    </style>\r\n    <style type=\"text/css\" emogrify=\"no\">\r\n        @media (max-width: 600px) {{\r\n            .gmx-killpill {{ content: ' \\03D1';}}\r\n            /* ... Estilos para dispositivos móveis ... */\r\n        }}\r\n    </style>\r\n    <style type=\"text/css\" emogrify=\"no\">\r\n        /* ... Outros estilos específicos ... */\r\n        /* ... Estilos adicionais ... */\r\n    </style>\r\n</head>\r\n<body bgcolor=\"#ffffff\" text=\"#3b3f44\" link=\"#696969\" yahoo=\"fix\" style=\"background-color: #ffffff;\">\r\n    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" class=\"nl2go-body-table\" width=\"100%\" style=\"background-color: #ffffff; width: 100%;\">\r\n        <tr>\r\n            <td>\r\n                <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"left\" class=\"r0-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                    <tr>\r\n                        <td valign=\"top\" class=\"r1-i\" style=\"background-color: #ffffff;\">\r\n                            <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"center\" class=\"r3-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                <tr>\r\n                                    <td class=\"r4-i\" style=\"padding-top: 20px;\">\r\n                                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                            <tr>\r\n                                                <th width=\"100%\" valign=\"top\" class=\"r5-c\" style=\"font-weight: normal;\">\r\n                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" class=\"r6-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                        <tr>\r\n                                                            <td valign=\"top\" class=\"r7-i\">\r\n                                                                <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                                                    <tr>\r\n                                                                        <td class=\"r8-c\" align=\"right\">\r\n                                                                            <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"right\" class=\"r9-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                                                <tr>\r\n                                                                                    <td valign=\"top\">\r\n                                                                                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                                                                            <tr>\r\n                                                                                                <td class=\"r10-c\" align=\"left\">\r\n                                                                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" class=\"r0-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                                                                        <tr>\r\n                                                                                                            <td align=\"left\" valign=\"top\" class=\"r11-i nl2go-default-textstyle\" style=\"color: #3b3f44; font-family: arial,helvetica,sans-serif; font-size: 16px; line-height: 1.5; word-break: break-word; padding-top: 15px; text-align: left;\">\r\n                                                                                                                <div>\r\n                                                                                                                    <p style=\"margin: 0;\">\r\n                                                                                                                        <span style=\"font-family: Arial; font-size: 18px;\"><strong>Email para recuperação de senha</strong></span><br>\r\n                                                                                                                        Para recuperar sua senha <a href='{resetPasswordEmail}' target='_blank' style='color: #696969; text-decoration: underline;'><span style=\"color: #0C5CC4;\">Clique Aqui</span></a>\r\n                                                                                                                        <br>\r\n                                                                                                                        &nbsp;\r\n                                                                                                                    </p>\r\n                                                                                                                </div>\r\n                                                                                                            </td>\r\n                                                                                                        </tr>\r\n                                                                                                    </table>\r\n                                                                                                </td>\r\n                                                                                            </tr>\r\n                                                                                            <tr>\r\n                                                                                                <td class=\"r10-c\" align=\"left\">\r\n                                                                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"30%\" class=\"r0-o\" style=\"border-collapse: separate; border-radius: 6px; table-layout: fixed; width: 30%;\">\r\n                                                                                                        <tr>\r\n                                                                                                            <td class=\"r12-i\" style=\"border-radius: 6px; padding-bottom: 15px; padding-top: 15px;\">\r\n                                                                                                                <img src=\"https://img.mailinblue.com/6642671/images/content_library/original/6560fe04e8ace31da7ff1c02.png\" width=\"\" alt=\"Nada\" border=\"0\" style=\"display: block; width: 100%; border-radius: 6px;\">\r\n                                                                                                            </td>\r\n                                                                                                        </tr>\r\n                                                                                                    </table>\r\n                                                                                                </td>\r\n                                                                                            </tr>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </table>\r\n                                                                        </td>\r\n                                                                    </tr>\r\n                                                                </table>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </th>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n"
        );
    }


    private static string RandomStringGeneration(int length) {

        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
        return new string(
            Enumerable.Repeat(chars, length)
            .Select(
                s => s[random.Next(s.Length)]
            ).ToArray()
        );

    }


    private async Task<Domain.Entities.User?> Validate(RequestEmail request) {

        var result = new ForgotPasswordValidator().Validate(request);
        var userEmailExists = await _userManager.FindByEmailAsync(request.Email);

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        } else {
            if (userEmailExists is null) {
                throw new UnaunthorizedException(
                    ResourceErrorMessages.EMAIL_NAO_REGISTRADO
                );
            } else {
                if (!userEmailExists.IsVerified) {
                    throw new UnaunthorizedException(
                        ResourceErrorMessages.VERIFICAR_EMAIL
                    );
                }
            }

        }

       

        return userEmailExists;
    }

}
