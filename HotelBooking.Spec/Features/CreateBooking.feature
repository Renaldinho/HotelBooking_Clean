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
          
    

    Scenario Outline: Valid date but no rooms available, boundary value testing
        
        Given the start date is <daysFromNow> days in the future from today
        And the end date is <daysAfterStart> days in the future from today
        And there are 0 rooms available during period
        When the customer creates a booking
        Then the booking should be not created
        
        Examples:
          | daysFromNow | daysAfterStart |
          | 1           | 2              |
          | 2           | 3              |
          | 21          | 28             |
          | 729         | 730            |
          | 730         | 731            |
          | 1           | 2              |
          

    Scenario Outline: Invalid date but rooms available. (dates in the past)
        Given the start date is <daysInThePastStarting> days in the past from today
        And the end date is <daysInThePastEnding> days in the past from today
        And there are <roomsAvailable> rooms available during period
        When the customer creates a booking
        Then the booking should create an exception
        
        Examples: 
        | daysInThePastStarting | daysInThePastEnding | roomsAvailable |
        | 0                     | 0                   | 10             |
        | 1                     | 10                  | 3              |
        | 14                    | 12                  | 4              |
        | 740                   | 701                 | 4              |
        | 750                   | 702                 | 4              |
        
    Scenario Outline: Invalid date but rooms available (end date before start date)
        Given the start date is <startDays> days in the future from today
        And the end date is <endDays> days in the future from today
        And there are <roomsAvailable> rooms available during period
        When the customer creates a booking
        Then the booking should create an exception

        Examples:
          | startDays | endDays | roomsAvailable |
          | 2         | 1       | 1              |
          | 10        | 5       | 3              |
          | 730       | 729     | 4              |

    Scenario Outline: Invalid date and no rooms available (start date in the past)
        Given the start date is <startDays> days in the past from today
        And the end date is <endDays> days in the future from today
        And there are 0 rooms available during period
        When the customer creates a booking
        Then the booking should create an exception

        Examples:
          | startDays | endDays |
          | 1         | 2       |
          | 10        | 15      | 

    Scenario Outline: Invalid date and no rooms available (end date before start date)
        Given the start date is <startDays> days in the future from today
        And the end date is <endDays> days in the future from today
        And there are 0 rooms available during period
        When the customer creates a booking
        Then the booking should create an exception

        Examples:
          | startDays | endDays |
          | 2         | 1       |
          | 10        | 5       |
        
        