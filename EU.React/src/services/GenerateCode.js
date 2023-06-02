import request from '@/utils/request';

export async function GenerateCode(code) {

    return request('/api/SmAutoCode/GenerateCode', {
        params: {
            code
        },
    });
}