import { request } from '@umijs/max';
import '../../style.css';
import { IAPI, IEditor } from '../../typings';

type BloggerType = {
    blogger: {
        blogId: string;
        postId: string;
    }
}

class Blogger {

    data: BloggerType;
    api: IAPI;
    bloggers: any;

    constructor(props: IEditor<BloggerType>) {
        this.data = props.data
        this.api = props.api;
    }

    static get toolbox() {
        return {
            title: 'Blogger',
            icon: '📰'
        };
    }

    makeSelectHolder() : Node {
        const select = document.createElement('select');
        select.classList.add(this.api.styles.input);
        request(`/setting/blogger/blog-list`).then((response: any[]) => {
            this.bloggers = response;
            select.id = 'blogId';
            if (response && response.length > 0) {
                let html = '';
                response.forEach(option => {
                    html += `<option value="${option.id}">${option.name}</option>`
                });
                select.innerHTML = html;
                select.value = this.data?.blogger?.blogId ?? '';
            }
            return select;
        });
        return select;
    }

    render() {

        const wrapper = document.createElement('div');
        wrapper.classList.add('form-group');

        const inputPostId = document.createElement('input');
        inputPostId.placeholder = 'Post Id'
        inputPostId.id = 'postId';
        inputPostId.value = this.data?.blogger?.postId ?? '';
        inputPostId.classList.add(this.api.styles.input);

        const selectHolder = this.makeSelectHolder();
        wrapper.appendChild(selectHolder);
        wrapper.appendChild(inputPostId);

        return wrapper as any;
    }

    save(e: any) {
        const blogId = e.querySelector('#blogId').value;
        const postId = e.querySelector('#postId').value;
        return {
            blogger: { blogId, postId }
        }
    }
}

export default Blogger