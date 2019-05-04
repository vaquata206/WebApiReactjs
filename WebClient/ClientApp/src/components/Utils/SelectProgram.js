import React from 'react';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper } from "../../helpers/utils";
import { Spinner } from 'react-bootstrap';

class SelectProgram extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            apps: []
        };
    }

    componentWillMount() {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetUserApps).then(response => {
            this.setState({ apps: response.data });
        }).catch(error => {
            alertHelper.showError(error, "Lấy danh sách chương trình không thành công");
        }).then(() => {
            this.setState({ loading: false });
        });
    }

    render() {
        const { loading, apps } = this.state;
        if (loading) {
            return <Spinner className={this.props.className} animation="border" size="sm" />;
        } else {
            return (
                <select className={this.props.className} value={this.props.value || 0} onChange={this.props.onChange}>
                    <option value="0">Mặt định</option>
                    {
                        (apps || []).map(value => <option key={value.id_ChuongTrinh} value={value.id_ChuongTrinh}>{value.ten_ChuongTrinh}</option>)
                    }
                </select>);
        }
    }
}

export default SelectProgram;