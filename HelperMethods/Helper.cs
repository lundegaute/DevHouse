
namespace DevHouse.Helper {
    public static class ValidationHelper {
        public static void CheckIfExistsOrException(params object[] entities ){
            foreach(var entity in entities){
                if ( entity is null) {
                    throw new KeyNotFoundException("Not Found");
                }
            }
        }
        public static void IdInRangeOrException(int id, int tableId = 1){
            if ( id < tableId) {
                throw new IndexOutOfRangeException($"Id must be greater than: {tableId}");
            }
        }
    }
}