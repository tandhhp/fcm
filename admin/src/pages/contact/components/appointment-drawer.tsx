import { apiLeadByPhone, apiLeadDetailById } from "@/services/contact";
import { ProDescriptions } from "@ant-design/pro-components";
import { Drawer, Empty, Skeleton, Tag } from "antd";
import { useEffect, useState } from "react";

type AppointmentDrawerProps = {
  open: boolean;
  phoneNumber?: string;
  onOpenChange: (open: boolean) => void;
};

type LeadRow = {
  id: string;
  name?: string;
  phoneNumber?: string;
  eventDate?: string;
  status?: number;
  createdDate?: string;
  note?: string;
  address?: string;
  email?: string;
};

type LeadDetail = {
  id?: string;
  eventDate?: string;
  eventId?: string;
  name?: string;
  status?: number;
  address?: string;
  email?: string;
  branchId?: number;
  createdDate?: string;
  dateOfBirth?: string;
  gender?: boolean;
  identityNumber?: string;
  phoneNumber?: string;
  salesId?: string;
  eventName?: string;
};

type LeadAppointmentItem = LeadRow & LeadDetail;

const leadStatusMap: Record<number, string> = {
  0: "Chờ duyệt",
  1: "Đã duyệt",
  2: "Check-in",
  3: "Chốt deal",
  4: "Từ chối",
  5: "Mời lại"
};

const toDateString = (value?: string) => {
  if (!value) return "-";
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleString("vi-VN");
};

const AppointmentDrawer: React.FC<AppointmentDrawerProps> = ({ open, phoneNumber, onOpenChange }) => {
  const [loading, setLoading] = useState<boolean>(false);
  const [appointments, setAppointments] = useState<LeadAppointmentItem[]>([]);

  useEffect(() => {
    const loadAppointments = async () => {
      if (!open || !phoneNumber) {
        setAppointments([]);
        return;
      }

      setLoading(true);
      try {
        const listResult = await apiLeadByPhone(phoneNumber);
        const leadRows: LeadRow[] = listResult?.data || [];

        if (!leadRows.length) {
          setAppointments([]);
          return;
        }

        const detailResults = await Promise.all(
          leadRows.map(async (lead) => {
            try {
              const detailResult = await apiLeadDetailById(lead.id);
              return {
                ...lead,
                ...(detailResult?.data || {})
              } as LeadAppointmentItem;
            } catch {
              return lead as LeadAppointmentItem;
            }
          })
        );

        setAppointments(detailResults);
      } finally {
        setLoading(false);
      }
    };

    loadAppointments();
  }, [open, phoneNumber]);

  return (
    <Drawer
      title={`Chi tiết lịch hẹn${phoneNumber ? ` - ${phoneNumber}` : ""}`}
      width={760}
      open={open}
      onClose={() => onOpenChange(false)}
      destroyOnHidden
    >
      {loading ? (
        <Skeleton active paragraph={{ rows: 8 }} />
      ) : appointments.length === 0 ? (
        <Empty description="Không có dữ liệu lịch hẹn" />
      ) : (
        <div className="space-y-4">
          {appointments.map((item) => (
            <div key={item.id} className="rounded-md border border-gray-200 p-4">
              <div className="mb-3 flex items-center justify-between gap-2">
                <div className="text-base font-semibold">{item.name || "Không có tên"}</div>
                <Tag color="blue">{leadStatusMap[item.status ?? -1] || "Không xác định"}</Tag>
              </div>
                <ProDescriptions column={1} bordered size="small" labelStyle={{ width: 120 }}>
                    <ProDescriptions.Item label="Số điện thoại">{item.phoneNumber || "-"}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Sự kiện">{item.eventName || "-"}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Ngày hẹn" valueType="date">{item.eventDate}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Ngày tạo">{toDateString(item.createdDate)}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Email">{item.email || "-"}</ProDescriptions.Item>
                    <ProDescriptions.Item label="CCCD">{item.identityNumber || "-"}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Địa chỉ">{item.address || "-"}</ProDescriptions.Item>
                    <ProDescriptions.Item label="Ghi chú">{item.note || "-"}</ProDescriptions.Item>
                </ProDescriptions>
            </div>
          ))}
        </div>
      )}
    </Drawer>
  );
};

export default AppointmentDrawer;
