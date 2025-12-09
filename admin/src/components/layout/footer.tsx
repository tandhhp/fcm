import { HomeOutlined, MessageOutlined } from "@ant-design/icons";
import { DefaultFooter } from "@ant-design/pro-components";
import { FloatButton, Drawer } from "antd";
import { useState } from "react";
import ChatAssistant from "../chat-assistant/ChatAssistant";

const Footer: React.FC = () => {

    const [openChat, setOpenChat] = useState<boolean>(false);

    return (
        <>
            <DefaultFooter copyright="2025 First Class Membership. All rights reserved." links={[
                {
                    key: 'Waffle',
                    title: <HomeOutlined />,
                    href: 'https://1stclass.com.vn',
                    blankTarget: true,
                },
                {
                    key: 'Waffle',
                    title: `Trang chủ`,
                    href: 'https://1stclass.com.vn',
                    blankTarget: true,
                }
            ]} />
            <FloatButton.Group>
                <FloatButton icon={<MessageOutlined />} tooltip="Trợ lý ảo" onClick={() => setOpenChat(true)} />
            </FloatButton.Group>
            <Drawer
            width={600}
                title="Trợ lý ảo AI"
                placement="right"
                onClose={() => setOpenChat(false)}
                open={openChat}
            >
                <ChatAssistant onClose={() => setOpenChat(false)} />
            </Drawer>
        </>
    );
};

export default Footer;