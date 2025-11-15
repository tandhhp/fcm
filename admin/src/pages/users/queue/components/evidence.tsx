import { Drawer } from "antd"
import { DrawerProps } from "antd/lib"

type Props = DrawerProps & {
    data?: any;
}

const Evidence : React.FC<Props> = (props) => {
    return (
        <Drawer {...props} title={`Chứng cứ: ${props.data?.name || ''}`} width={800} destroyOnClose>
            <div dangerouslySetInnerHTML={{ __html: props.data?.evidence || '' }} />
        </Drawer>
    )
}

export default Evidence