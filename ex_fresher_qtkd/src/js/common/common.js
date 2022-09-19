
export default {
    formatEmptyDate(){

    },
    /**
     * Format date trước khi hiển thị lên màn hình
     * Author: HMHieu(19/09/2022)
     *  */

    formatDate(dateSrc) {
        try {
            if(dateSrc){
                let date = new Date(dateSrc);
                date = date.toISOString().substring(0, 10);
                return date;
            }      
        } catch (error) {
            console.log(error);
    
        }
    },
     /**
     * Xét khoảng được chọn trong date
     * Author: HMHieu(19/09/2022)
     *  */
    maxDate() {       
        try {        
            let date = new Date(),
            year = date.getFullYear().toString(),
            month = (date.getMonth() + 1).toString().padStart(2, '0'),
            day = date.getDate().toString().padStart(2, '0');
            return `${year}-${month}-${day}`;
            
          
        } catch (error) {
            console.log(error)
        }

    },

}
