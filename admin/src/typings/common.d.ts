declare namespace API {
    type PagedResult<T> = {
        data: T[];
        total: number;
    }
    type FilterOptions = {
        current: number;
        pageSize: number;
        [key: string]: any;
    }
    type TResult<T> = {
        succeeded: boolean;
        data: T;
        message?: string;
    }
}