import { ArrowUpOutlined } from "@ant-design/icons";
import { Button } from "antd";
import WfUpload from "./upload";
import { useState } from "react";

const ButtonUpload: React.FC = () => {
    const [open, setOpen] = useState<boolean>(false);
    return (
        <>
            <Button
                icon={<ArrowUpOutlined />}
                type="primary"
                onClick={() => setOpen(true)}
            >
                Upload
            </Button>
            <WfUpload open={open} onCancel={() => setOpen(false)} onFinish={() => setOpen(true)} />
        </>
    )
}

export default ButtonUpload;