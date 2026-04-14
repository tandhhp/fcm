export default (initialState: { currentUser?: API.User }) => {
  const { currentUser } = initialState ?? {};

  /** Quản trị viên */
  const canAdmin = currentUser && currentUser.roles.includes('admin');
  
  const canCX = currentUser && (currentUser.roles.includes('cx') || currentUser.roles.includes('cxtp') || canAdmin);

  const canAccountant = currentUser && (currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant') || currentUser.roles.includes('admin'));

  const canCXM = currentUser && (currentUser.roles.includes('cxtp') || currentUser.roles.includes('admin'));

  const cxm = currentUser && currentUser.roles.includes('cxtp');
  const cx = currentUser && currentUser.roles.includes('cx');

  /** Quản lý nhân viên (HR) */
  const canHR = currentUser && (currentUser.roles.includes('hr') || currentUser.roles.includes('admin'));

  const canComment = currentUser && (currentUser.roles.includes('cx') 
  || currentUser.roles.includes('cxtp')
  || currentUser.roles.includes('admin'));

  // Quản lý form đăng ký
  const canForm = currentUser && (currentUser.roles.includes('cx')
  || currentUser.roles.includes('accountant')
  || currentUser.roles.includes('ChiefAccountant')
  || currentUser.roles.includes('cxtp')
  || currentUser.roles.includes('admin'));

  // Quản lý điểm
  const canDeposit = currentUser && (currentUser.roles.includes('cx')
  || currentUser.roles.includes('cxtp'));
  // Quản lý chủ thẻ
  const canCardHolder = currentUser && (currentUser.roles.includes('cx')
  || currentUser.roles.includes('cxtp')
  || currentUser.roles.includes('sales')
  || currentUser.roles.includes('accountant')
  || currentUser.roles.includes('ChiefAccountant')
  || currentUser.roles.includes('dos')
  || currentUser.roles.includes('sm')
  || currentUser.roles.includes('admin'));
  // Thêm sửa, xóa chủ thẻ
  const canCRUDCardHolder = currentUser && (currentUser.roles.includes('cx')
  || currentUser.roles.includes('cxtp'));
  const canApproveComment = currentUser && (currentUser.roles.includes('cxtp')
  || currentUser.roles.includes('admin'));

  const canSales = currentUser && (currentUser.roles.includes('sales')
  || currentUser.roles.includes('sm')
  || currentUser.roles.includes('dos')
  || currentUser.roles.includes('admin'));

  const canCreateEmployee = currentUser && (currentUser.roles.includes('hr')
  || currentUser.roles.includes('admin'));

  const canDos = currentUser && (currentUser.roles.includes('dos') || currentUser.roles.includes('admin'));

  const canSm = currentUser && (currentUser.roles.includes('sm')
  || currentUser.roles.includes('admin'));

  const canDosAccountant = currentUser && (currentUser.roles.includes('dos')
  || currentUser.roles.includes('ChiefAccountant')
  || currentUser.roles.includes('accountant')
  || currentUser.roles.includes('admin'));

  const canViewChart = currentUser && (currentUser.roles.includes('accountant')
  || currentUser.roles.includes('ChiefAccountant')
  || currentUser.roles.includes('sm')
  || currentUser.roles.includes('dos')
  || currentUser.roles.includes('admin'));

  /** Giám đốc kinh doanh */
  const dos = currentUser && currentUser.roles.includes('dos');
  /** Trưởng phòng kinh doanh */
  const sm = currentUser && (currentUser.roles.includes('sm'));
  /** Nhân viên kinh doanh */
  const sales = currentUser && (currentUser.roles.includes('sales'));
  /** Nhân viên sự kiện */
  const event = currentUser && (currentUser.roles.includes('event') || canAdmin);
  /** Quản lý nhân viên sự kiện */
  const em = currentUser && (currentUser.roles.includes('em') || canAdmin);
  /** Telesales */
  const telesale = currentUser && (currentUser.roles.includes('Telesale'));
  /** Quản lý Telesales */
  const telesaleManager = currentUser && (currentUser.roles.includes('TelesaleManager'));
  /** Giám đốc Telesales */
  const dot = currentUser && currentUser.roles.includes('dot');
  /** Nhân sự */
  const hr = currentUser && currentUser.roles.includes('hr');
  /** Kế toán */
  const accountant = currentUser && (currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant'));
  /** Kế toán trưởng */
  const chiefAccountant = currentUser && currentUser.roles.includes('ChiefAccountant');
  /** Quản trị viên dữ liệu */
  const adminData = currentUser && currentUser.roles.includes('admindata');
  /** Quản lý pháp lý */
  const legalExcutive = currentUser && currentUser.roles.includes('legalexecutive');
  /** Trợ lý kinh doanh */
  const salesAdmin = currentUser && currentUser.roles.includes('salesadmin');

  const canEvent = currentUser && (sales || currentUser.roles.includes('event') || sm || dos || canAdmin);

  const canLead = canSales ||
   (currentUser && (currentUser.roles.includes('Telesale') || currentUser.roles.includes('TelesaleManager') || currentUser.roles.includes('event') 
    || dot))
  
  const canCardHolderQueue = currentUser && (currentUser.roles.includes('ChiefAccountant')
  || currentUser.roles.includes('cx')
  || currentUser.roles.includes('cxtp')
  || currentUser.roles.includes('dos')
  || currentUser.roles.includes('admin')
  || currentUser.roles.includes('event')
  || currentUser.roles.includes('sales')
  || currentUser.roles.includes('sm')
  || currentUser.roles.includes('accountant'));

  const canTelesales = currentUser && (telesale || telesaleManager || dot || canAdmin || adminData);

  const canContract = currentUser && (sales || sm || dos || legalExcutive || canAdmin || salesAdmin || currentUser.roles.includes('cxtp') || currentUser.roles.includes('cx') || currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant'));

  const can_read_page_finance = accountant || chiefAccountant || canAdmin || cx || salesAdmin;
  const can_read_page_finance_invoice = accountant || chiefAccountant || canAdmin || salesAdmin;
  const can_read_page_finance_bill = accountant || chiefAccountant || canAdmin;
  const can_read_page_finance_report = accountant || chiefAccountant || canAdmin;
  const can_read_page_finance_sales_admin = salesAdmin || canAdmin;
  const can_read_page_contact_source = adminData || dot || canAdmin;
  const can_read_page_event_voucher = event || em || canAdmin;
  const can_read_page_event_contract = event || em || canAdmin;
  const can_read_page_event_customer = dos || dot || event || canAdmin || em;
  const can_read_page_event_checkin = event || em || canAdmin;
  const can_read_page_event_invoice = sm || dot || dos || event || canAdmin || em;
  const can_read_page_event = event || sales || sm || dos || dot || telesaleManager || telesale || cx || canAdmin || em;
  const can_read_page_event_showup_report = event || canAdmin || dot || dos || sm || cx || sales || em;
  const can_confirm2 = currentUser && (currentUser.claims?.some((claim) => claim.type === 'ACCESS' && claim.value === 'CONFIRM2') || false) || canAdmin;
  const can_read_page_user = canAdmin || hr || dot || adminData;
  const can_read_page_department = canAdmin || hr || dot || adminData;
  const can_read_page_report = canAdmin || dot || adminData;
  const can_read_page_report_data_source = can_read_page_report;
  const can_read_page_report_tmr_data = can_read_page_report;
  const can_read_page_report_multiple_assign = can_read_page_report;

  return {
    canAdmin,
    canCX,
    canCXM,
    canAccountant,
    canHR,
    canComment,
    canForm,
    canDeposit,
    canCardHolder,
    canCRUDCardHolder,
    canApproveComment,
    canSales,
    canCreateEmployee,
    canDos,
    canSm,
    canDosAccountant,
    canViewChart,
    canEvent,
    sales,
    event,
    em,
    canCardHolderQueue,
    cx,
    cxm,
    dos,
    canLead,
    telesale,
    telesaleManager,
    hr,
    dot,
    accountant,
    chiefAccountant,
    sm,
    legalExcutive,
    salesAdmin,
    adminData,
    canTelesales,
    canContract,
    can_read_page_finance,
    can_read_page_finance_bill,
    can_read_page_finance_invoice,
    can_read_page_finance_report,
    can_read_page_finance_sales_admin,
    can_read_page_contact_source,
    can_read_page_event,
    can_read_page_event_voucher,
    can_read_page_event_contract,
    can_read_page_event_customer,
    can_read_page_event_invoice,
    can_read_page_event_showup_report,
    can_read_page_event_checkin,
    can_confirm2,
    can_read_page_user,
    can_read_page_department,
    can_read_page_report,
    can_read_page_report_data_source,
    can_read_page_report_tmr_data,
    can_read_page_report_multiple_assign
  };
};
