import { Observable } from "rxjs";

export interface IRepositoryBase<TEntity> {
    findAll(): Observable<Array<TEntity>>;
    create(entity: TEntity): Observable<TEntity>;
    update(entity: TEntity): Observable<TEntity>;
    find(id: string | number): Observable<TEntity>;
}