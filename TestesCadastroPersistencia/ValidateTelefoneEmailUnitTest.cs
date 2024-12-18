using CoreContato;
using System.Text.RegularExpressions;

namespace TestesCadastroPersistencia
{
    public class ValidateTelefoneEmailUnitTest
    {
        [Theory]
        [InlineData("Barbara", "barbara@email.com", "12345678901", true)]
        [InlineData("Debora", "debora@email.com", "09876543210", true)]
        [InlineData("", "debora@email.com", "12345678901", false)]
        [InlineData("Samuel", "qualquercoisa", "12345678901", false)]
        [InlineData("Vitoria", "Vitoria@email.com", "1234567890", false)]
        public void ValidateContato_ValidatesInputCorrectly(string nome, string email, string telefone, bool expectedIsValid)
        {
            var contato = new Contato
            {
                nome_contato = nome,
                email_contato = email,
                telefone_contato = telefone
            };

            var isValid = true;

            try
            {
                ValidateContato(contato);
            }
            catch (ArgumentException)
            {
                isValid = false;
            }

            Assert.Equal(expectedIsValid, isValid);
        }

        //[Theory]
        //[InlineData("barbara@email.com", "12345678901", true)]
        //[InlineData("invalid-email", "12345678901", false)]
        //[InlineData("debora@email.com", "1234567890", false)]
        //[InlineData("", "", false)]
        //public void ValidateUpdateContato_ValidatesInputCorrectly(string email, string telefone, bool expectedIsValid)
        //{
        //    var contato = new Contato
        //    {
        //        email_contato = email,
        //        telefone_contato = telefone
        //    };

        //    var isValid = true;

        //    try
        //    {
        //        ValidateUpdateContato(contato);
        //    }
        //    catch (ArgumentException)
        //    {
        //        isValid = false;
        //    }

        //    Assert.Equal(expectedIsValid, isValid);
        //}

        private void ValidateContato(Contato contato)
        {
            if (string.IsNullOrEmpty(contato.nome_contato))
            {
                throw new ArgumentException("Nome do contato é obrigatório.");
            }

            if (!IsValidEmail(contato.email_contato))
            {
                throw new ArgumentException("E-mail inválido.");
            }

            if (!IsValidTelefone(contato.telefone_contato))
            {
                throw new ArgumentException("Telefone inválido.");
            }
        }

        //private void ValidateUpdateContato(Contato existingContato)
        //{
        //    if (!IsValidEmail(existingContato.email_contato))
        //    {
        //        throw new ArgumentException("E-mail inválido.");
        //    }

        //    if (!IsValidTelefone(existingContato.telefone_contato))
        //    {
        //        throw new ArgumentException("Telefone inválido.");
        //    }
        //}

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidTelefone(string telefone)
        {
            var regex = new Regex(@"^\d{11}$");
            return regex.IsMatch(telefone);
        }
    }
}