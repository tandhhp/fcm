import { HomeOutlined, MessageOutlined } from "@ant-design/icons";
import { DefaultFooter } from "@ant-design/pro-components";
import { FloatButton, Drawer } from "antd";
import { useState } from "react";
import ChatAssistant from "../chat-assistant/ChatAssistant";
import dayjs from "dayjs";

const { COMPANY_NAME } = process.env;

const Footer: React.FC = () => {

    const [openChat, setOpenChat] = useState<boolean>(false);

    return (
        <>
            <DefaultFooter copyright={`${dayjs().year()} ${COMPANY_NAME}. All rights reserved.`} links={[
                {
                    key: 'home-icon',
                    title: <HomeOutlined />,
                    href: 'https://defzone.net',
                    blankTarget: true,
                },
                {
                    key: 'home-text',
                    title: `Trang chủ`,
                    href: 'https://defzone.net',
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