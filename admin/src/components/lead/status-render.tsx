import { LeadStatus } from "@/utils/constants";

const LeadStatusRender: React.FC<{ status: LeadStatus }> = ({ status }) => {
    if (status === LeadStatus.Pending) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-yellow-100 text-yellow-800">Chờ duyệt</div>
    }
    if (status === LeadStatus.Approved) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-green-100 text-green-800">Đã duyệt</div>
    }
    if (status === LeadStatus.Checkin) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-blue-100 text-blue-800">Check-in</div>
    }
    if (status === LeadStatus.CloseDeal) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-purple-100 text-purple-800">Chốt deal</div>
    }
    if (status === LeadStatus.LeadReject) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-red-100 text-red-800">Từ chối</div>
    }
    if (status === LeadStatus.ReInvite) {
        return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-orange-100 text-orange-800">Mời lại</div>
    }
    return <div className="px-2 py-[2px] text-xs rounded font-bold w-full text-center bg-gray-100 text-gray-800">Không xác định</div>
}

export default LeadStatusRender;