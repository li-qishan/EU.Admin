using EU.Core;
using EU.Core.Utilities;
using EU.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;

namespace EU.Common
{
    public class IVChangeHelper
    {
        public static void Add(MaterialIVChange change, IDbTransaction trans = null)
        {
            DbInsert di = new DbInsert("BdMaterialIVChange");
            di.Values("MaterialId", change.MaterialId);
            di.Values("StockId", change.StockId);
            di.Values("GoodsLocationId", change.GoodsLocationId);
            di.Values("QTY", change.QTY);
            di.Values("BeforeQTY", change.BeforeQTY);
            di.Values("AfterQTY", change.AfterQTY);
            di.Values("ChangeType", change.ChangeType);
            di.Values("OrderId", change.OrderId);
            di.Values("OrderDetailId", change.OrderDetailId);
            DBHelper.Instance.ExecuteDML(di.GetSql(), null, null, trans);
        }

        /// <summary>
        /// 获取物料库存
        /// </summary>
        /// <param name="MaterialId">物料ID</param>
        /// <param name="StockId">仓库ID</param>
        /// <param name="GoodsLocationId">货位ID</param>
        /// <param name="trans">trans</param>
        /// <returns></returns>
        public static decimal GetMaterialInventory(Guid? MaterialId, Guid? StockId, Guid? GoodsLocationId, IDbTransaction trans = null)
        {
            decimal QTY = 0;
            string sql = @"SELECT *
                            FROM BdMaterialInventory
                            WHERE MaterialId = '{0}'
                                  AND StockId = '{1}'
                                  AND GoodsLocationId = '{2}'";
            sql = string.Format(sql, MaterialId, StockId, GoodsLocationId);
            BdMaterialInventory iv = DBHelper.Instance.QueryTransFirst<BdMaterialInventory>(sql, null, null, trans);
            if (iv != null)
                QTY = iv.QTY;
            else
            {
                DbInsert di = new DbInsert("BdMaterialInventory");
                di.Values("MaterialId", MaterialId);
                di.Values("StockId", StockId);
                di.Values("GoodsLocationId", GoodsLocationId);
                di.Values("QTY", 0);
                DBHelper.Instance.ExecuteDML(di.GetSql(), null, null, trans);
            }

            return QTY;
        }

        public enum ChangeType
        {
            /// <summary>
            /// 销售出库
            /// </summary>
            SalesOut,
            /// <summary>
            /// 销售退货
            /// </summary>
            SalesReturn,
            /// <summary>
            /// 采购入库
            /// </summary>
            PurchaseIn,
            /// <summary>
            /// 采购退货
            /// </summary>
            PurchaseReturn,
            /// <summary>
            /// 库存调整
            /// </summary>
            InventoryAdjust,
            /// <summary>
            /// 库存调拨
            /// </summary>
            InventoryTransfers,
            /// <summary>
            /// 库存盘点
            /// </summary>
            InventoryCheck,
            /// <summary>
            /// 库存其他入库单
            /// </summary>
            InventoryOtherIn,
            /// <summary>
            /// 库存其他出库单
            /// </summary>
            InventoryOtherOut,
            /// <summary>
            /// 库存初始化
            /// </summary>
            InventoryInit
        }
    }
}
