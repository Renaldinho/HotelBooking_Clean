Feature: Create Booking

    Scenario Outline: Valid date period and room availability, boundary value testing

        Given the start date is <daysFromNow> days in the future from today
        And the end date is <daysAfterStart> days in the future from today
        And there are <availableRooms> rooms available during period
        When the customer creates a booking
        Then the booking should be created successfully

        Examples:
          | daysFromNow | daysAfterStart | availableRooms |
          | 1           | 2              | 1              | 
          | 2           | 3              | 2              | 
          | 21          | 28             | 5              |
          | 729         | 730            | 10             | 
          | 730         | 731            | 1              | 
          | 1           | 2              | 3              | 