declare namespace API {
    interface ContractServiceUsage {
        id: string;
        contractId: string;
        serviceName: string;
        usedDate: string;
        amount: number;
        peopleCount: number;
        createdDate: string;
        modifiedDate: string;
    }
    interface ContractLeaveVoucherUsage {
        id: string;
        contractId: string;
        voucherName: string;
        usedDate: string;
        amount: number;
        peopleCount: number;
        createdDate: string;
        modifiedDate: string;
    }
}