import { apiInvoiceExport } from "@/services/finances/invoice";
import { ExportOutlined } from "@ant-design/icons";
import { useAccess } from "@umijs/max";
import { Button } from "antd";
import { useState } from "react";

type Props = {
    exportOptions?: any;
}

export const InvoiceExportForm: React.FC<Props> = ({ exportOptions }) => {

    const access = useAccess();
    const [loading, setLoading] = useState<boolean>(false);

    const handleExport = async () => {
        setLoading(true);
        const response = await apiInvoiceExport(exportOptions);
        const url = window.URL.createObjectURL(new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `invoices.xlsx`);
        document.body.appendChild(link);
        link.click();
        link.parentNode?.removeChild(link);
        setLoading(false);
    }

    return (
        <>
            <Button type="primary" icon={<ExportOutlined />}
                loading={loading}
                onClick={handleExport} disabled={access.sales || access.telesale || access.sm || access.telesaleManager}>
                Xuất phiếu thu
            </Button>
        </>
    )
}

export default InvoiceExportForm;