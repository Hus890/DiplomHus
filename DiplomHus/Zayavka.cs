//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiplomHus
{
    using System;
    using System.Collections.Generic;
    
    public partial class Zayavka
    {
        public int ID_Zayavka { get; set; }
        public int ID_User { get; set; }
        public int ID_Type { get; set; }
        public int ID_Oboryduvanie { get; set; }
        public int ID_MestoRemonta { get; set; }
        public int ID_Status { get; set; }
        public string Phone { get; set; }
        public System.DateTime Date { get; set; }
        public string Opisanie { get; set; }
        public string ComentsFinishWork { get; set; }
        public Nullable<int> MasterName { get; set; }
    
        public virtual MestoRemonta MestoRemonta { get; set; }
        public virtual Oboryduvanie Oboryduvanie { get; set; }
        public virtual Status Status { get; set; }
        public virtual Type Type { get; set; }
        public virtual User User { get; set; }
    }
}
