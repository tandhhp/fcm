declare namespace CPN {
    type BaseComponent = {
        id?: string;
    };
    type Jumbotron = BaseComponent & {
        backgroundImage: string;
    };
    type Header = BaseComponent & {
        viewName?: string;
        logo?: string;
        brand?: string;
        templates: string[];
    }
    type ProductLister = BaseComponent & {
        title?: string;
        itemPerRow?: string;
        pageSize?: number;
    }
    type ProductPicker = BaseComponent & {
        title?: string;
        tagIds?: string[];
    }
    type Brand = BaseComponent & {
        name: string;
        logo: string;
        url: string;
    }
    type Sponsor = BaseComponent & {
        brands?: Brand[];
    }
    type Link = BaseEntity & {
        name: string;
        href: string;
        target: string;
    };
    type ListGroupItem = BaseEntity & {
        link: Link;
        icon?: string;
        badge?: number;
        suffix?: string;
    };
    type ListGroup = BaseComponent & {
        name: string;
        items: ListGroupItem[];
    };
    type ShopeeProduct = BaseComponent & {
        title: string;
        urlSuffix: string;
        groupId: string;
    };
    type ProductSpotlight = BaseComponent & {
        title: string;
        pageSize: number;
        itemPerRow: string;
    };
}