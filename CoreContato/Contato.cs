using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;
using KeyAttribute = Dapper.Contrib.Extensions.KeyAttribute;

namespace CoreContato
{
    [Table("Contatos")]
    public class Contato
    {
        [ExplicitKey]
        [Column("id_contato")]
        public int id_contato { get; set; }

        [Column("nome_contato")]
        public string nome_contato { get; set; }

        [Column("telefone_contato")]
        public string telefone_contato { get; set; }

        [Column("email_contato")]
        public string email_contato { get; set; }

        public Contato()
        {
            
        }

        public Contato(int id, string nome, string telefone, string email)
        {
            id_contato = id;
            nome_contato = nome;
            telefone_contato = telefone;
            email_contato = email;
            
        }



    }

}
