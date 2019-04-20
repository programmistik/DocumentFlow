using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string DocNumber { get; set; }
        public DateTime DocDate { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public int DocumentStateId { get; set; }
        public virtual DocumentState DocumentState { get; set; }
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        // Organization contacts here //
        public DateTime CreateDate { get; set; }
        public int CrUserId { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int MdUserId { get; set; }
        public virtual User ModifyBy { get; set; }

        public string Comment { get; set; }

        public virtual ICollection<MyFile> myFiles { get; set; }

        public Document()
        {
            myFiles = new HashSet<MyFile>();
        }

    }
}
