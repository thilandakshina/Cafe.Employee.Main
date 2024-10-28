export const GenderType = {
    Male: 0,
    Female: 1,
    Other: 2
  };
  
  // Helper function to get gender label
  export const getGenderLabel = (value) => {
    switch (value) {
      case GenderType.Male:
        return 'Male';
      case GenderType.Female:
        return 'Female';
      case GenderType.Other:
        return 'Other';
      default:
        return '';
    }
  };