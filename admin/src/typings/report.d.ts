// Các chỉ số chung xuất hiện ở tất cả các cấp độ
export interface BaseMetrics {
    contactImport: number;
    total: number;
    cF1: number;
    showup: number;
    deal: number;
    percentCFTotalContacted: number;
    percentShowupCF: number;
    percentDealShowup: number;
}

// Cấp độ chi tiết nhất (bên trong mảng sources)
export interface Source extends BaseMetrics {
    sourceName: string;
}

// Cấp độ đội nhóm (bên trong mảng teams)
export interface Team extends BaseMetrics {
    sourceGroup: string;
    sourceName: string;
    sources: Source[];
}

// Cấp độ tổng cộng (object total)
export interface TotalSummary extends BaseMetrics {
    sourceGroup: string;
    contactStartCase: number;
    teleNotUpdate: number;
}

// Type tổng quát cho toàn bộ object data
export interface ReportData {
    total: TotalSummary;
    teams: Team[];
}

// Interface cho đối tượng User (Nhân viên)
export interface TMRUser {
    userId: string;
    userName: string;
    fullName: string;
    totalAssign: number;
    totalAvailableContact: number;
    // callStatusCounts đang là object rỗng {}, thông thường nó sẽ là dạng key-value đếm số lượng (vd: { "answered": 10, "busy": 2 })
    // Sử dụng Record<string, number> là phù hợp nhất, hoặc thay bằng 'any' nếu cấu trúc phức tạp hơn
    callStatusCounts: Record<string, number>; 
}

// Interface cho đối tượng Team (Nhóm)
export interface TMRTeam {
    teamId: number;
    leaderName: string;
    totalAssign: number;
    totalAvailableContact: number;
    callStatusCounts: {
        tele: number; // Tele not update
        temp: number; // Temporary locked/Wrong number/Knm
        notEnough: number; // Not Enough Qualify
        meet: number; // Meet Require
        refuse: number; // Refuse to talk
        location: number; // Location
    }
    users: TMRUser[];
}

// Interface tổng thể cho toàn bộ Object
export interface TMRViewReportData {
    viewType: number;
    teams: TMRTeam[];
}