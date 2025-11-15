import { DeleteOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Typography } from 'antd';

type ImagePreviewProps = {
  src: API.FileContent;
  onClick?: any;
  onRemove?: any;
};

const ImagePreview: React.FC<ImagePreviewProps> = (props) => {
  const onConfirm = () => {
    if (props.onRemove) {
      props.onRemove(props.src);
    }
  };
  return (
    <div className="flex">
      <div
        style={{
          padding: '.5rem 2rem',
          backgroundColor: '#efefef',
          position: 'relative',
          width: 200,
          height: 150,
          display: 'flex',
          justifyContent: 'center',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Popconfirm title="Are you sure?" onConfirm={onConfirm}>
          <Button
            icon={<DeleteOutlined />}
            size="small"
            type="link"
            danger
            style={{
              position: 'absolute',
              right: 0,
              top: 0,
            }}
          />
        </Popconfirm>
        <div className="text-center">
          <img
            style={{
              maxHeight: 180,
              maxWidth: 100,
            }}
            src={props.src.url}
            alt={props.src.name}
            onClick={props.onClick}
            className="cursor-pointer"
          />
        </div>
        <Typography.Text className="truncate">{props.src.name}</Typography.Text>
      </div>
    </div>
  );
};

export default ImagePreview;
