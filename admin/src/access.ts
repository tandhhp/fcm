export default (initialState: { currentUser?: API.User }) => {
  const { currentUser } = initialState ?? {};
  const canAdmin = currentUser && currentUser.roles.includes('admin');
  
  const canCX = currentUser && (currentUser.roles.includes('cx') || currentUser.roles.includes('cxtp') || canAdmin);

  const canAccountant = currentUser && (currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant') || currentUser.roles.includes('admin'));

  const canCXM = currentUser && (currentUser.roles.includes('cxtp') || currentUser.roles.includes('admin'));

  const cxm = currentUser && currentUser.roles.includes('cxtp');
  const cx = currentUser && currentUser.roles.includes('cx');

  // Quản lý nhân viên
  const canHR = currentUser && (currentUser.roles.includes('hr') 
  || currentUser.roles.includes('dos')
  || currentUser.roles.includes('admin'));

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

  const dos = currentUser && currentUser.roles.includes('dos');
  const sm = currentUser && (currentUser.roles.includes('sm') || canAdmin);
  const sales = currentUser && (currentUser.roles.includes('sales'));
  const event = currentUser && (currentUser.roles.includes('event') || canAdmin);
  const telesale = currentUser && (currentUser.roles.includes('Telesale'));
  const telesaleManager = currentUser && (currentUser.roles.includes('TelesaleManager'));
  const dot = currentUser && currentUser.roles.includes('dot');
  const hr = currentUser && (currentUser.roles.includes('hr') || currentUser.roles.includes('admin'));
  const accountant = currentUser && (currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant'));
  const chiefAccountant = currentUser && currentUser.roles.includes('ChiefAccountant');
  const adminData = currentUser && currentUser.roles.includes('admindata');

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

  const canContract = currentUser && (sales || sm || dos || canAdmin || currentUser.roles.includes('cxtp') || currentUser.roles.includes('cx') || currentUser.roles.includes('accountant') || currentUser.roles.includes('ChiefAccountant'));

  const can_read_page_finance = accountant || chiefAccountant || canAdmin || cx;
  const can_read_page_contact_source = adminData || dot || canAdmin;
  const can_read_page_event_customer = dos || dot || event || canAdmin;
  const can_read_page_event_invoice = sm || dot || dos || event || canAdmin;
  const can_read_page_event = event || sales || sm || dos || dot || telesaleManager || telesale || cx || canAdmin;
  const can_read_page_event_showup_report = event || canAdmin || dot || dos || sm || cx || sales;
  const can_confirm2 = currentUser && (currentUser.claims?.some((claim) => claim.type === 'ACCESS' && claim.value === 'CONFIRM2') || false) || canAdmin;

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
    adminData,
    canTelesales,
    canContract,
    can_read_page_finance,
    can_read_page_contact_source,
    can_read_page_event,
    can_read_page_event_customer,
    can_read_page_event_invoice,
    can_read_page_event_showup_report,
    can_confirm2
  };
};
