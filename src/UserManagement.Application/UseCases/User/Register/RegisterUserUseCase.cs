using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Services.Token;
using UserManagement.Communication.Requests;
using UserManagement.Communication.Response;
using UserManagement.Domain.Repositories;
using UserManagement.Domain.Repositories.EmailRepository;
using UserManagement.Domain.Repositories.UserRepository;
using UserManagement.Exceptions;
using UserManagement.Exceptions.ExceptionsBase;

namespace UserManagement.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase {

    private readonly IUserReadOnlyRepository _repositoryRead;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenService _tokenService;
    private readonly ISendEmail _sendEmail;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public RegisterUserUseCase(
        IUserReadOnlyRepository repositoryRead,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        TokenService tokenService,
        ISendEmail sendEmail,
        UserManager<Domain.Entities.User> userManager
    ){
        _repositoryRead = repositoryRead;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _sendEmail = sendEmail;
        _userManager = userManager;
    }


    public async Task<ResponseTokens> Execute(RequestRegisterUser request) {

        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        var passwordToken = RandomStringGeneration(70);

        user.Email = request.Email;
        user.UserName = request.Email;
        user.Name = request.Name;
        user.CreationDate = NormalizeDatetimeToSouthAmerica();
        user.VerifyLoginToken = passwordToken;
        user.PasswordTokenExpirationDate = null;
        user.VerifyLoginTokenExpirationDate = NormalizeDatetimeToSouthAmerica().AddMinutes(30);

        await _userManager.CreateAsync(user, request.Password);
        await _unitOfWork.Commit();

        SendEmailOnRegister(request.Email, passwordToken);

        var result = await _tokenService.GenerateJwtToken(user);

        return new ResponseTokens { 
            Access_Token = result.Access_Token,
            Refresh_Token = result.Refresh_Token 
        };

    }


    private static DateTime NormalizeDatetimeToSouthAmerica() {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }


    private void SendEmailOnRegister(
        string email,
        string passwordToken
    ){
        var origin = "http://localhost:3000";

        var verifyEmail = $"{origin}/verify-email?passwordToken={passwordToken}";

        _sendEmail.Send(
            emailsTo: email,
            subject: "E-mail para verificação",
            message: $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"format-detection\" content=\"telephone=no\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>E-mail para recuperação de senha</title>\r\n    <style type=\"text/css\">\r\n        #outlook a {{ padding:0; }}\r\n        .ExternalClass {{ width:100%; }}\r\n        .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div {{ line-height: 100%; }}\r\n        table td {{ border-collapse: collapse; mso-line-height-rule: exactly; }}\r\n        .editable.image {{ font-size: 0 !important; line-height: 0 !important; }}\r\n        .nl2go_preheader {{ display: none !important; mso-hide:all !important; mso-line-height-rule: exactly; visibility: hidden !important; line-height: 0px !important; font-size: 0px !important; }}\r\n        body {{ width:100% !important; -webkit-text-size-adjust:100%; -ms-text-size-adjust:100%; margin:0; padding:0; }}\r\n        img {{ outline:none; text-decoration:none; -ms-interpolation-mode: bicubic; }}\r\n        a img {{ border:none; }}\r\n        table {{ border-collapse:collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; }}\r\n        th {{ font-weight: normal; text-align: left; }}\r\n        *[class=\"gmail-fix\"] {{ display: none !important; }}\r\n    </style>\r\n    <style type=\"text/css\" emogrify=\"no\">\r\n        @media (max-width: 600px) {{\r\n            .gmx-killpill {{ content: ' \\03D1';}}\r\n            /* ... Estilos para dispositivos móveis ... */\r\n        }}\r\n    </style>\r\n    <style type=\"text/css\" emogrify=\"no\">\r\n        /* ... Outros estilos específicos ... */\r\n        /* ... Estilos adicionais ... */\r\n    </style>\r\n</head>\r\n<body bgcolor=\"#ffffff\" text=\"#3b3f44\" link=\"#696969\" yahoo=\"fix\" style=\"background-color: #ffffff;\">\r\n    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" class=\"nl2go-body-table\" width=\"100%\" style=\"background-color: #ffffff; width: 100%;\">\r\n        <tr>\r\n            <td>\r\n                <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"left\" class=\"r0-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                    <tr>\r\n                        <td valign=\"top\" class=\"r1-i\" style=\"background-color: #ffffff;\">\r\n                            <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"center\" class=\"r3-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                <tr>\r\n                                    <td class=\"r4-i\" style=\"padding-top: 20px;\">\r\n                                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                            <tr>\r\n                                                <th width=\"100%\" valign=\"top\" class=\"r5-c\" style=\"font-weight: normal;\">\r\n                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" class=\"r6-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                        <tr>\r\n                                                            <td valign=\"top\" class=\"r7-i\">\r\n                                                                <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                                                    <tr>\r\n                                                                        <td class=\"r8-c\" align=\"right\">\r\n                                                                            <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" align=\"right\" class=\"r9-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                                                <tr>\r\n                                                                                    <td valign=\"top\">\r\n                                                                                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\">\r\n                                                                                            <tr>\r\n                                                                                                <td class=\"r10-c\" align=\"left\">\r\n                                                                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"100%\" class=\"r0-o\" style=\"table-layout: fixed; width: 100%;\">\r\n                                                                                                        <tr>\r\n                                                                                                            <td align=\"left\" valign=\"top\" class=\"r11-i nl2go-default-textstyle\" style=\"color: #3b3f44; font-family: arial,helvetica,sans-serif; font-size: 16px; line-height: 1.5; word-break: break-word; padding-top: 15px; text-align: left;\">\r\n                                                                                                                <div>\r\n                                                                                                                    <p style=\"margin: 0;\">\r\n                                                                                                                        <span style=\"font-family: Arial; font-size: 18px;\"><strong>Email para recuperação de senha</strong></span><br>\r\n                                                                                                                        Para recuperar sua senha <a href='{verifyEmail}' target='_blank' style='color: #696969; text-decoration: underline;'><span style=\"color: #0C5CC4;\">Clique Aqui</span></a>\r\n                                                                                                                        <br>\r\n                                                                                                                        &nbsp;\r\n                                                                                                                    </p>\r\n                                                                                                                </div>\r\n                                                                                                            </td>\r\n                                                                                                        </tr>\r\n                                                                                                    </table>\r\n                                                                                                </td>\r\n                                                                                            </tr>\r\n                                                                                            <tr>\r\n                                                                                                <td class=\"r10-c\" align=\"left\">\r\n                                                                                                    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\" width=\"30%\" class=\"r0-o\" style=\"border-collapse: separate; border-radius: 6px; table-layout: fixed; width: 30%;\">\r\n                                                                                                        <tr>\r\n                                                                                                            <td class=\"r12-i\" style=\"border-radius: 6px; padding-bottom: 15px; padding-top: 15px;\">\r\n                                                                                                                <img src=\"https://img.mailinblue.com/6642671/images/content_library/original/6560fe04e8ace31da7ff1c02.png\" width=\"\" alt=\"Nada\" border=\"0\" style=\"display: block; width: 100%; border-radius: 6px;\">\r\n                                                                                                            </td>\r\n                                                                                                        </tr>\r\n                                                                                                    </table>\r\n                                                                                                </td>\r\n                                                                                            </tr>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </table>\r\n                                                                        </td>\r\n                                                                    </tr>\r\n                                                                </table>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </th>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n"
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


    private async Task Validate(RequestRegisterUser request) {

        var result = new RegisterUserValidator().Validate(request);

        var userEmailExists = await _repositoryRead
            .UserEmailExists(request.Email);

        if (userEmailExists) {
            result.Errors.Add(
                new FluentValidation.Results.ValidationFailure(
                    "email",
                    ResourceErrorMessages.EMAIL_JA_REGISTRADO
                )
            );
        }

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(erro => erro.ErrorMessage).ToList();

            throw new ValidationErrorsException(errorMessages);
        }

    }

}
