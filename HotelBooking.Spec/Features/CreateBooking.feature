Feature: Create Booking

    Scenario Outline: Valid date period, boundary value testing

        Given the start date is <daysFromNow> days in the future from today
        And the end date is <daysAfterStart> days in the future from today
        And there is a room available during period
        When the customer creates a booking
        Then the booking should be created successfully

        Examples:
          | daysFromNow | daysAfterStart | 
          | 1           | 2              |
          | 2           | 3              |
          | 21          | 28             |
          | 729         | 730            | 
          | 730         | 731            |