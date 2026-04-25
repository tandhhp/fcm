export enum InvoiceStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3,
    SAConfirmed = 4
}

export enum PaymentMethod {
    BankTransfer = 0,
    Card = 1,
    Cash = 2
}

export enum BillStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}

export enum CONFIRM2_STATUS {
    UNCONFIRM = 0,
    CONFIRM = 1,
    REJECT = 2,
    NOT_SURE = 3,
    NO_ANSWER = 4
}