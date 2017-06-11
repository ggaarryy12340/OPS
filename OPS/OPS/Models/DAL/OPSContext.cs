﻿namespace OPS.Models.DAL
{
    using Models.OPSContext;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class OPSContext : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'OPSContext' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'OPS.Models.DAL.OPSContext' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'OPSContext' 連接字串。
        public OPSContext()
            : base("name=OPSContext")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderStatusLog> OrderStatusLog { get; set; }
        public virtual DbSet<PickingMakeLog> PickingMakeLog { get; set; }
        public virtual DbSet<Picking> Picking { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<PDCategory> PDCategory { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}