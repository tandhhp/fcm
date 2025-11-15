import { HomeOutlined, MessageOutlined } from "@ant-design/icons";
import { DefaultFooter } from "@ant-design/pro-components";
import { history } from "@umijs/max";
import { FloatButton } from "antd";

const Footer: React.FC = () => {
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
                <FloatButton icon={<MessageOutlined />} tooltip="Trợ lý ảo" onClick={() => history.push('/chat')} />
            </FloatButton.Group>
        </>
    );
};

export default Footer;