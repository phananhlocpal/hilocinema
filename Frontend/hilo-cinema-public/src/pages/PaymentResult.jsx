import { Box, Heading, Text } from '@chakra-ui/react';
import { CheckCircleIcon, WarningIcon } from '@chakra-ui/icons';
import { useLocation } from 'react-router-dom';

const PaymentResult = () => {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    
    const success = queryParams.get('success');
    
    const isSuccess = success === "True";

    return (
        <Box textAlign="center" py={10} px={6}>
            {isSuccess ? (
                <>
                    <CheckCircleIcon boxSize={'50px'} color={'green'} />
                    <Heading as="h2" size="xl" mt={6} mb={2}>
                        Thanh toán thành công
                    </Heading>
                    <Text color={'gray.500'}>
                        Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. Đơn hàng của bạn đã được xử lý thành công.
                    </Text>
                </>
            ) : (
                <>
                    <WarningIcon boxSize={'50px'} color={'red.500'} />
                    <Heading as="h2" size="xl" mt={6} mb={2}>
                        Thanh toán thất bại
                    </Heading>
                    <Text color={'gray.500'}>
                        Rất tiếc, đã có sự cố xảy ra trong quá trình xử lý thanh toán. Vui lòng thử lại.
                    </Text>
                </>
            )}
        </Box>
    );
};

export default PaymentResult;
