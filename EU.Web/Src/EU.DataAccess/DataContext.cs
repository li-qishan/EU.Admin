using EU.Domain.System;
using Microsoft.EntityFrameworkCore;
using System;
using EU.Model.BFProject;
using EU.Model.BFProject.Payment;
using EU.Model.System;
using EU.Model.System.CompanyStructure;
using EU.Model.System.Privilege;
using EU.Model.System.Setup;
using EU.Model.System.WorkFlow;
using EU.Model;

namespace EU.DataAccess
{
    public class DataContext : DbContext
    {
        #region 流程
        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<BFContract> Contract { get; set; }
        public virtual DbSet<Payplan> Payplan { get; set; }
        public virtual DbSet<InvoiceRegist> InvoiceRegist { get; set; }
        public virtual DbSet<CompletionFinalAccount> CompletionFinalAccount { get; set; }
        public virtual DbSet<ProjectImplement> ProjectImplement { get; set; }
        public virtual DbSet<BeforeImplement> BeforeImplement { get; set; }
        public virtual DbSet<ImplementProcess> ImplementProcess { get; set; }
        public virtual DbSet<ApplyPay> ApplyPay { get; set; }
        #endregion

        #region 系统相关
        public virtual DbSet<SmModule> SmModules { get; set; }
        public virtual DbSet<SmRole> SmRoles { get; set; }
        public virtual DbSet<SmRoleModule> SmRoleModule { get; set; }

        public virtual DbSet<SmModuleSql> SmModuleSql { get; set; }
        public virtual DbSet<SmModuleColumn> SmModuleColumn { get; set; }
        public virtual DbSet<SmUser> SmUsers { get; set; }
        public virtual DbSet<SmUserRole> SmUserRole { get; set; }
        public virtual DbSet<SmLov> SmLov { get; set; }
        public virtual DbSet<SmLovDetail> SmLovDetail { get; set; }
        public virtual DbSet<SmAutoCode> SmAutoCode { get; set; }
        public virtual DbSet<FileAttachment> FileAttachment { get; set; }
        public virtual DbSet<SmCompany> SmCompany { get; set; }
        public virtual DbSet<SmDepartment> SmDepartment { get; set; }
        public virtual DbSet<SmSchedule> SmSchedule { get; set; }
        public virtual DbSet<SmFunctionPrivilege> SmFunctionPrivilege { get; set; }
        public virtual DbSet<SmRoleFunction> SmRoleFunction { get; set; }

        /// <summary>
        /// 系统参数分组
        /// </summary>
        public virtual DbSet<SmConfigGroup> SmConfigGroup { get; set; }

        /// <summary>
        /// 系统参数配置
        /// </summary>
        public virtual DbSet<SmConfig> SmConfig { get; set; }

        public virtual DbSet<SmNode> SmNodes { get; set; }
        public virtual DbSet<SmEdge> SmEdges { get; set; }
        public virtual DbSet<SmProjectFlow> SmProjectFlow { get; set; }
        public virtual DbSet<SmEmployee> SmEmployee { get; set; }

        /// <summary>
        /// 系统表字典
        /// </summary>
        public virtual DbSet<SmTableCatalog> SmTableCatalog { get; set; }

        /// <summary>
        /// 系统表栏位
        /// </summary>
        public virtual DbSet<SmFieldCatalog> SmFieldCatalog { get; set; }

        /// <summary>
        /// 导入模板定义
        /// </summary>
        public virtual DbSet<SmImpTemplate> SmImpTemplate { get; set; }

        /// <summary>
        /// 导入模板定义明细
        /// </summary>
        public virtual DbSet<SmImpTemplateDetail> SmImpTemplateDetail { get; set; }

        /// <summary>
        /// 导入数据明细
        /// </summary>
        public virtual DbSet<SmImportDataDetail> SmImportDataDetail { get; set; }

        /// <summary>
        /// API接口授权
        /// </summary>
        public virtual DbSet<SmApi> SmApi { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public virtual DbSet<SmProvince> SmProvince { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public virtual DbSet<SmCity> SmCity { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public virtual DbSet<SmCounty> SmCounty { get; set; }

        /// <summary>
        /// 微信用户
        /// </summary>
        public virtual DbSet<WxUser> WxUser { get; set; }

        /// <summary>
        //任务管理
        /// </summary>
        public virtual DbSet<SmQuartzJob> SmQuartzJob { get; set; }

        #endregion

        #region 微信管理

        /// <summary>
        /// 微信配置
        /// </summary>
        public virtual DbSet<WxConfig> WxConfig { get; set; }

        /// <summary>
        /// 微信菜单
        /// </summary>
        public virtual DbSet<WxMenu> WxMenu { get; set; }


        /// <summary>
        /// 微信菜单明细
        /// </summary>
        public virtual DbSet<WxMenuDetail> WxMenuDetail { get; set; }

        #endregion

        #region 基础档案
        public virtual DbSet<MaterialType> BdMaterialType { get; set; }
        public virtual DbSet<Unit> BdUnit { get; set; }
        public virtual DbSet<Texture> BdTexture { get; set; }
        public virtual DbSet<Color> BdColor { get; set; }
        public virtual DbSet<Stock> BdStock { get; set; }
        public virtual DbSet<GoodsLocation> BdGoodsLocation { get; set; }
        public virtual DbSet<Material> BdMaterial { get; set; }
        public virtual DbSet<DeliveryWay> BdDeliveryWay { get; set; }
        public virtual DbSet<Currency> BdCurrency { get; set; }
        public virtual DbSet<SettlementWay> BdSettlementWay { get; set; }
        public virtual DbSet<Customer> BdCustomer { get; set; }
        public virtual DbSet<CustomerDeliveryAddress> BdCustomerDeliveryAddress { get; set; }
        public virtual DbSet<CustomerContact> BdCustomerContact { get; set; }
        public virtual DbSet<CustomerInvoice> BdCustomerInvoice { get; set; }
        public virtual DbSet<Supplier> BdSupplier { get; set; }

        /// <summary>
        /// 物料库存
        /// </summary>
        public virtual DbSet<BdMaterialInventory> BdMaterialInventory { get; set; }

        #endregion

        #region 销售管理

        /// <summary>
        /// 销售单
        /// </summary>
        public virtual DbSet<Order> SdOrder { get; set; }
        /// <summary>
        /// 销售单明细
        /// </summary>
        public virtual DbSet<OrderDetail> SdOrderDetail { get; set; }

        /// <summary>
        /// 销售单预付账款
        /// </summary>
        public virtual DbSet<OrderPrepayment> SdOrderPrepayment { get; set; }
        public virtual DbSet<ShipOrder> SdShipOrder { get; set; }
        public virtual DbSet<ShipOrderDetail> SdShipOrderDetail { get; set; }
        public virtual DbSet<OutOrder> SdOutOrder { get; set; }
        public virtual DbSet<OutOrderDetail> SdOutOrderDetail { get; set; }
        public virtual DbSet<ReturnOrder> SdReturnOrder { get; set; }
        public virtual DbSet<ReturnOrderDetail> SdReturnOrderDetail { get; set; }


        #endregion

        #region 采购管理

        /// <summary>
        /// 请购单
        /// </summary>
        public virtual DbSet<Requestion> PoRequestion { get; set; }

        /// <summary>
        /// 请购单明细
        /// </summary>
        public virtual DbSet<RequestionDetail> PoRequestionDetail { get; set; }

        /// <summary>
        /// 采购单
        /// </summary>
        public virtual DbSet<POOrder> PoOrder { get; set; }

        /// <summary>
        /// 采购单明细
        /// </summary>
        public virtual DbSet<POOrderDetail> PoOrderDetail { get; set; }

        /// <summary>
        /// 采购单预付账款
        /// </summary>
        public virtual DbSet<PoOrderPrepayment> PoOrderPrepayment { get; set; }

        /// <summary>
        /// 采购到货通知单
        /// </summary>
        public virtual DbSet<ArrivalOrder> PoArrivalOrder { get; set; }
        public virtual DbSet<ArrivalOrderDetail> PoArrivalOrderDetail { get; set; }
        public virtual DbSet<InOrder> PoInOrder { get; set; }
        public virtual DbSet<InOrderDetail> PoInOrderDetail { get; set; }
        public virtual DbSet<POReturnOrder> PoReturnOrder { get; set; }
        public virtual DbSet<POReturnOrderDetail> PoReturnOrderDetail { get; set; }

        /// <summary>
        /// 采购扣款单
        /// </summary>
        public virtual DbSet<PoDeductionOrder> PoDeductionOrder { get; set; }

        /// <summary>
        /// 采购扣款单明细
        /// </summary>
        public virtual DbSet<PoDeductionDetail> PoDeductionDetail { get; set; }

        /// <summary>
        /// 采购费用单
        /// </summary>
        public virtual DbSet<PoFeeOrder> PoFeeOrder { get; set; }

        /// <summary>
        /// 采购费用单明细
        /// </summary>
        public virtual DbSet<PoFeeDetail> PoFeeDetail { get; set; }
        #endregion

        #region 仓库管理

        /// <summary>
        /// 库存初始化
        /// </summary>
        public virtual DbSet<IvInit> IvInit { get; set; }

        /// <summary>
        /// 库存初始化明细
        /// </summary>
        public virtual DbSet<IvInitDetail> IvInitDetail { get; set; }


        /// <summary>
        /// 库存调整单
        /// </summary>
        public virtual DbSet<IvAdjust> IvAdjust { get; set; }

        /// <summary>
        /// 库存调整单明细
        /// </summary>
        public virtual DbSet<IvAdjustDetail> IvAdjustDetail { get; set; }

        /// <summary>
        /// 库存调拨单
        /// </summary>
        public virtual DbSet<IvTransfers> IvTransfers { get; set; }

        /// <summary>
        /// 库存调拨单明细
        /// </summary>
        public virtual DbSet<IvTransfersDetail> IvTransfersDetail { get; set; }

        /// <summary>
        /// 库存盘点单
        /// </summary>
        public virtual DbSet<IvCheck> IvCheck { get; set; }

        /// <summary>
        /// 库存调拨单明细
        /// </summary>
        public virtual DbSet<IvCheckDetail> IvCheckDetail { get; set; }

        /// <summary>
        /// 其他入库单
        /// </summary>
        public virtual DbSet<IvOtherIn> IvOtherIn { get; set; }

        /// <summary>
        /// 其他入库单明细
        /// </summary>
        public virtual DbSet<IvOtherInDetail> IvOtherInDetail { get; set; }

        /// <summary>
        /// 其他出库单
        /// </summary>
        public virtual DbSet<IvOtherOut> IvOtherOut { get; set; }

        /// <summary>
        /// 其他出库单明细
        /// </summary>
        public virtual DbSet<IvOtherOutDetail> IvOtherOutDetail { get; set; }

        /// <summary>
        /// 实际盘点单
        /// </summary>
        public virtual DbSet<IvActualCheck> IvActualCheck { get; set; }

        /// <summary>
        /// 实际盘点单明细
        /// </summary>
        public virtual DbSet<IvActualCheckDetail> IvActualCheckDetail { get; set; }


        #endregion

        #region 设备管理

        /// <summary>
        /// 设备分类
        /// </summary>
        public virtual DbSet<MachineType> EmMachineType { get; set; }

        /// <summary>
        /// 设备基础资料
        /// </summary>
        public virtual DbSet<Machine> EmMachine { get; set; }

        #endregion

        #region 产品结构

        /// <summary>
        /// 设备分类
        /// </summary>
        public virtual DbSet<WorkShop> PsWorkShop { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public virtual DbSet<Process> PsProcess { get; set; }

        /// <summary>
        /// 工序机台
        /// </summary>
        public virtual DbSet<ProcessMachine> PsProcessMachine { get; set; }

        /// <summary>
        /// 工序外协厂商
        /// </summary>
        public virtual DbSet<ProcessSupplier> PsProcessSupplier { get; set; }

        /// <summary>
        /// 工序单价
        /// </summary>
        public virtual DbSet<ProcessPrice> PsProcessPrice { get; set; }

        /// <summary>
        /// 工序人员
        /// </summary>
        public virtual DbSet<ProcessEmployee> PsProcessEmployee { get; set; }

        /// <summary>
        /// 工序不良原因
        /// </summary>
        public virtual DbSet<ProcessBadReason> PsProcessBadReason { get; set; }

        /// <summary>
        /// 工序模板
        /// </summary>
        public virtual DbSet<ProcessTemplate> PsProcessTemplate { get; set; }

        /// <summary>
        /// 工序模板明细
        /// </summary>
        public virtual DbSet<ProcessTemplateDetail> PsProcessTemplateDetail { get; set; }

        /// <summary>
        /// 工序模板物料
        /// </summary>
        public virtual DbSet<ProcessTemplateMaterial> PsProcessTemplateMaterial { get; set; }

        /// <summary>
        /// BOM
        /// </summary>
        public virtual DbSet<BOM> PsBOM { get; set; }

        /// <summary>
        /// BOM物料
        /// </summary>
        public virtual DbSet<BOMMaterial> PsBOMMaterial { get; set; }


        /// <summary>
        /// BOM工模治具
        /// </summary>
        public virtual DbSet<BOMMould> PsBOMMould { get; set; }

        /// <summary>
        /// BOM工艺路线
        /// </summary>
        public virtual DbSet<BOMProcess> PsBOMProcess { get; set; }

        #endregion

        #region 工模治具
        /// <summary>
        /// 工模治具
        /// </summary>
        public virtual DbSet<Mould> MfMould { get; set; }

        /// <summary>
        /// 工模治具类别
        /// </summary>
        public virtual DbSet<MouldType> MfMouldType { get; set; }

        /// <summary>
        /// 工模治具入账
        /// </summary>
        public virtual DbSet<MfInOrder> MfInOrder { get; set; }

        /// <summary>
        /// 工模治具入账明细
        /// </summary>
        public virtual DbSet<MfInOrderDetail> MfInOrderDetail { get; set; }
        #endregion

        #region 生产管理
        /// <summary>
        /// 生产工单
        /// </summary>
        public virtual DbSet<PdOrder> PdOrder { get; set; }

        /// <summary>
        /// 生产工单-材料明细
        /// </summary>
        public virtual DbSet<PdOrderMaterial> PdOrderMaterial { get; set; }

        /// <summary>
        /// 生产工单-对应订单
        /// </summary>
        public virtual DbSet<PdOrderDetail> PdOrderDetail { get; set; }

        /// <summary>
        /// 生产工单工艺路线
        /// </summary>
        public virtual DbSet<PdOrderProcess> PdOrderProcess { get; set; }

        /// <summary>
        /// 生产工单工模治具
        /// </summary>
        public virtual DbSet<PdOrderMould> PdOrderMould { get; set; }

        /// <summary>
        /// 生产计划工单
        /// </summary>
        public virtual DbSet<PdPlanOrder> PdPlanOrder { get; set; }

        /// <summary>
        /// 生产计划工单明细
        /// </summary>
        public virtual DbSet<PdPlanDetail> PdPlanDetail { get; set; }

        /// <summary>
        /// 需求分析工单
        /// </summary>
        public virtual DbSet<PdRequireOrder> PdRequireOrder { get; set; }

        /// <summary>
        /// 需求工单分析
        /// </summary>
        public virtual DbSet<PdRequireAnalysis> PdRequireAnalysis { get; set; }

        /// <summary>
        /// 材料补发工单
        /// </summary>
        public virtual DbSet<PdReissueOrder> PdReissueOrder { get; set; }

        /// <summary>
        /// 材料补发工单明细
        /// </summary>
        public virtual DbSet<PdReissueDetail> PdReissueDetail { get; set; }

        /// <summary>
        /// 材料出库工单
        /// </summary>
        public virtual DbSet<PdOutOrder> PdOutOrder { get; set; }

        /// <summary>
        /// 材料出库工单明细
        /// </summary>
        public virtual DbSet<PdOutDetail> PdOutDetail { get; set; }

        /// <summary>
        /// 材料退库工单
        /// </summary>
        public virtual DbSet<PdReturnOrder> PdReturnOrder { get; set; }

        /// <summary>
        /// 材料退库工单明细
        /// </summary>
        public virtual DbSet<PdReturnDetail> PdReturnDetail { get; set; }

        /// <summary>
        /// 材料完工工单
        /// </summary>
        public virtual DbSet<PdCompleteOrder> PdCompleteOrder { get; set; }

        /// <summary>
        /// 产品完工入库明细
        /// </summary>
        public virtual DbSet<PdCompleteDetail> PdCompleteDetail { get; set; }

        #endregion

        #region 应付管理

        /// <summary>
        /// 应付期初建账
        /// </summary>
        public virtual DbSet<ApInitAccountOrder> ApInitAccountOrder { get; set; }

        /// <summary>
        /// 应付期初建账明细
        /// </summary>
        public virtual DbSet<ApInitAccountDetail> ApInitAccountDetail { get; set; }

        /// <summary>
        /// 应付对账单
        /// </summary>
        public virtual DbSet<ApCheckOrder> ApCheckOrder { get; set; }

        /// <summary>
        /// 应付对账单明细
        /// </summary>
        public virtual DbSet<ApCheckDetail> ApCheckDetail { get; set; }

        /// <summary>
        /// 应付发票单
        /// </summary>
        public virtual DbSet<ApInvoiceOrder> ApInvoiceOrder { get; set; }

        /// <summary>
        /// 应付对应发票
        /// </summary>
        public virtual DbSet<ApInvoiceAssociation> ApInvoiceAssociation { get; set; }

        /// <summary>
        /// 应付发票单明细
        /// </summary>
        public virtual DbSet<ApInvoiceDetail> ApInvoiceDetail { get; set; }

        /// <summary>
        /// 采购付款单明细
        /// </summary>
        public virtual DbSet<ApPaymentDetail> ApPaymentDetail { get; set; }

        /// <summary>
        /// 采购付款单
        /// </summary>
        public virtual DbSet<ApPaymentOrder> ApPaymentOrder { get; set; }

        /// <summary>
        /// 采购付款核销明细
        /// </summary>
        public virtual DbSet<ApPaymentWriteOff> ApPaymentWriteOff { get; set; }

        /// <summary>
        /// 采购预付款
        /// </summary>
        public virtual DbSet<ApPrepaidOrder> ApPrepaidOrder { get; set; }

        /// <summary>
        /// 采购预付款明细
        /// </summary>
        public virtual DbSet<ApPrepaidDetail> ApPrepaidDetail { get; set; }

        #endregion

        #region 应收管理
        /// <summary>
        /// 应收对账单
        /// </summary>
        public virtual DbSet<ArCheckOrder> ArCheckOrder { get; set; }

        /// <summary>
        /// 应收对账单明细
        /// </summary>
        public virtual DbSet<ArCheckDetail> ArCheckDetail { get; set; }

        /// <summary>
        /// 应收期初建账
        /// </summary>
        public virtual DbSet<ArInitAccountOrder> ArInitAccountOrder { get; set; }

        /// <summary>
        /// 应收期初建账明细
        /// </summary>
        public virtual DbSet<ArInitAccountDetail> ArInitAccountDetail { get; set; }

        /// <summary>
        /// 应收开票
        /// </summary>
        public virtual DbSet<ArInvoiceOrder> ArInvoiceOrder { get; set; }

        /// <summary>
        /// 应收开票明细
        /// </summary>
        public virtual DbSet<ArInvoiceDetail> ArInvoiceDetail { get; set; }

        /// <summary>
        /// 应收开票对应发票
        /// </summary>
        public virtual DbSet<ArInvoiceAssociation> ArInvoiceAssociation { get; set; }

        /// <summary>
        /// 销售收款单
        /// </summary>
        public virtual DbSet<ArSalesCollectionOrder> ArSalesCollectionOrder { get; set; }

        /// <summary>
        /// 销售收款单明细
        /// </summary>
        public virtual DbSet<ArSalesCollectionDetail> ArSalesCollectionDetail { get; set; }

        /// <summary>
        /// 销售收款核销明细
        /// </summary>
        public virtual DbSet<ArSalesCollectionWriteOff> ArSalesCollectionWriteOff { get; set; }

        /// <summary>
        /// 销售预收款
        /// </summary>
        public virtual DbSet<ArPrepaidOrder> ArPrepaidOrder { get; set; }

        /// <summary>
        /// 销售预收款明细
        /// </summary>
        public virtual DbSet<ArPrepaidDetail> ArPrepaidDetail { get; set; }


        #endregion

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
