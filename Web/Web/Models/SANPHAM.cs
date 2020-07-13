//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            this.CTDON_HANG = new HashSet<CTDON_HANG>();
            this.NHAP_KHO = new HashSet<NHAP_KHO>();
        }
    
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public Nullable<int> MaNSX { get; set; }
        public Nullable<int> MaLoai { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public Nullable<decimal> GiaNhap { get; set; }
        public string HinhMinhHoa { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string NoiDung { get; set; }
        public string ManHinh { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string CauHinh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDON_HANG> CTDON_HANG { get; set; }
        public virtual NHA_SAN_XUAT NHA_SAN_XUAT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHAP_KHO> NHAP_KHO { get; set; }
        public virtual PHAN_LOAI PHAN_LOAI { get; set; }
    }
}