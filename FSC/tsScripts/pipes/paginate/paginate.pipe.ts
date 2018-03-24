import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'paginate',
    pure: false
})
export class PaginatePipe implements PipeTransform {

    public transform(collection: any[], args: any): any {
        if (collection instanceof Array && collection.length > 0) {
            let start = (args.currentPage - 1) * args.pageSize;
            let end = start + args.pageSize;
            collection = collection.slice(start, end);
        }
        return collection;
    }
}
